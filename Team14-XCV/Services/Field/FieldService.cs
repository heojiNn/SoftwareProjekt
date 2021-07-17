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

        // for definition see   IFieldService
        public IEnumerable<Field> GetAllFields()
        {
            IEnumerable<Field> fields = new List<Field>();
            //-------------------------------------------------------------------------MS-SQL-Query
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                fields = con.Query<Field>("Select * From [Field]");
            }
            catch (SqlException e) { log.LogError($"GetAllFields() persistence Error: \n{e.Message}\n"); } //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            return fields;
        }




        //---write  Commands with validation-------------------------------------------------------
        // for definition see   IFieldService
        public int CreateField(Field toAdd, bool justValidate = false)
        {
            errorMessages = new();
            int changedRows = 0;
            toAdd.Name = toAdd.Name.Trim();
            //---------------------------------------------------------------------------Validation
            ValidateDataAno(toAdd);
            if (GetAllFields().Any(x => x.Name.ToLower() == toAdd.Name.ToLower()))
                errorMessages.Add($"({toAdd}), kann nicht hinzugefügt werden, da sie schon enthalten ist.");
            if (errorMessages.Any() || justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });                          //ErrorM
                return 0;
            }
            //-------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------MS-SQL-Command
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                changedRows += con.Execute("Insert Into [Field] Values (@Name)", toAdd);
            }
            catch (SqlException e) { log.LogError($"CreateField() persistence Error: \n{e.Message}\n"); } //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            OnChange(new() { SuccesMessage = $"({toAdd}), wurde zu den Branchen hinzugefügt." }); //SuccesM.
            return changedRows;
        }

        // for definition see   IFieldService
        public int DeleteField(Field toRemove)
        {
            int changedRows = 0;
            //---------------------------------------------------------------------------Validation
            if (!GetAllFields().Contains(toRemove))
            {
                OnChange(new() { ErrorMessages = new[] { $"({toRemove}), kann nicht entfernt werden, da sie nicht (mehr) enthalten ist." } });
                return 0;
            }
            //-------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------MS-SQL-Command
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                changedRows += con.Execute("Delete From [Field] Where [Name] = @Name", toRemove);
            }
            catch (SqlException e) { log.LogError($"DeleteField() persistence Error: \n{e.Message}\n"); } //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            OnChange(new() { SuccesMessage = $"({toRemove}), wurde aus den Branchen entfernt." }); //SuccesM.
            return changedRows;
        }

        // for definition see   IFieldService
        public (int added, int removed) UpdateAllFields(IEnumerable<Field> fields, bool justValidate = false)
        {
            errorMessages = new();
            int addedRows = 0;
            int removedRows = 0;
            //---------------------------------------------------------------------------Validation
            foreach (var field in fields)
            {
                field.Name = field.Name.Trim();
                ValidateDataAno(field);
            }
            if (errorMessages.Any() || justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return (0, 0);
            }
            //-------------------------------------------------------------------------------------
            var oldFields = GetAllFields();
            var toAdd = fields.Except(oldFields).ToList();
            var toRemove = oldFields.Except(fields).ToList();

            foreach (var a in toAdd)
                addedRows += CreateField(a);
            foreach (var r in toRemove)
                removedRows += DeleteField(r);

            var succes = "";
            if (addedRows > 5)
                succes += $"{addedRows} Branchen wurden hinzugefügt.\n";
            else if (addedRows > 0)
                succes += $"{string.Join(", ", toAdd)} wurden hinzugefügt.\n";
            if (removedRows > 5)
                succes += $"{removedRows} Branchen wurden entfernt.\n";
            else if (removedRows > 0)
                succes += $"{string.Join(", ", toRemove)} wurden entfernt.\n";
            OnChange(new() { SuccesMessage = succes });                         //SuccesMessage
            return (addedRows, removedRows);
        }


        /// <summary>
        ///         Validates against DataAnotation(.Name.Length <50 && >1)
        /// </summary>
        /// <remarks>
        ///         used by Update and Create
        /// </remarks>
        private IEnumerable<string> ValidateDataAno(Field field)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(field, new ValidationContext(field), results, true))
                foreach (var result in results)
                    errorMessages.Add($"{result.ErrorMessage} ({field})");
            return errorMessages;
        }


        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
