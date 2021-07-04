using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    /// <inheritdoc/>
    public class FieldService : IFieldService
    {
        private readonly string connectionString;
        private readonly ILogger<FieldService> log;

        public FieldService(IConfiguration config, ILogger<FieldService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }
        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   IFieldService
        public IEnumerable<string> ValidateFields(IEnumerable<Field> fields)
        {
            errorMessages = new();
            foreach (var field in fields)
            {
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(field, new ValidationContext(field), results, true))
                    foreach (var result in results)
                        errorMessages.Add($"{result.ErrorMessage} ({field})");
            }
            return errorMessages;
        }

        // for definition see   IFieldService
        public void CreateField(Field newField)
        {
            var oldFields = GetAllFields();
            if (oldFields.Contains(newField))

                OnChange(new() { ErrorMessages = new[] { $"({newField}): kann nicht hinzugefügt werden, da es schon enthalten ist." } });
            else
                UpdateAllFields(oldFields.Append(newField));
        }
        // for definition see   IFieldService
        public void RemoveField(Field field)
        {
            var oldFields = GetAllFields().ToList(); ;
            if (!oldFields.Contains(field))

                OnChange(new() { ErrorMessages = new[] { $"({field}): kann nicht entfernt werden, da es nicht enthalten ist." } });
            else
            {
                oldFields.Remove(field);
                UpdateAllFields(oldFields);
            }
        }





        //-------------------------------------Persistence-----------------------------------------
        //--read   --------------------------------------------------------------------------------
        // for definition see   IFieldService
        public IEnumerable<Field> GetAllFields()
        {
            IEnumerable<Field> fields = new List<Field>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                fields = con.Query<Field>("Select * From [Field]");
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllFields() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return fields;
        }
        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllFields(IEnumerable<Field> fields)
        {
            int addedRows = 0;
            int removedRows = 0;
            var oldFields = GetAllFields();
            var toAdd = fields.Except(oldFields);
            var toRemove = oldFields.Except(fields);
            ValidateFields(toAdd);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return (0, 0);    // if a new one(name) is invalid, nothing changes
            }

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                removedRows += con.Execute("Delete From [Field] Where [Name] = @Name", toRemove);
                addedRows += con.Execute("Insert Into [Field] Values (@Name)", toAdd);
            }
            catch (SqlException e)
            {
                log.LogError($"UpdateAllFields() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            var succes = "";
            if (toAdd.Any())
                succes += $"{string.Join(", ", toAdd)} wurde hinzugefügt.";
            if (toRemove.Any())
                succes += $"{string.Join(", ", toRemove)} wurde entfernt.";
            OnChange(new() { SuccesMessage = succes });
            return (addedRows, removedRows);
        }

        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
