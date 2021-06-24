using System.Collections.Generic;
using System.Collections.Immutable;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class FieldService : IFieldService
    {
        private readonly string connectionString;
        private readonly ILogger<FieldService> log;
        public FieldService(IConfiguration config, ILogger<FieldService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }
        //---------------------------------Buissines Logic-----------------------------------------
        // empty





        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Field> GetAllFields()
        {
            IEnumerable<Field> fields = new List<Field>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                fields = con.Query<Field>("Select * From Field");
                con.Close();
            }
            catch (SqlException e)
            {
                log.LogError($" retrieving Fields from database: {e.Message} \n");
            }
            return fields;
        }

        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllFields(IEnumerable<Field> fields)
        {
            int addedRows = 0;
            int removedRows = 0;
            var newFields = fields.ToImmutableSortedSet();
            var oldFields = GetAllFields().ToImmutableSortedSet();
            var toAdd = newFields.Except(oldFields);
            var toRemove = oldFields.Except(newFields);

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute("IF NOT EXISTS (  SELECT * FROM field  WHERE name = '' ) " +
                                        "Insert Into field Values ('') ;"); ;
                foreach (var field in toAdd)
                    addedRows += con.Execute("Insert Into Field Values (@Name)", new { field.Name });
                foreach (var field in toRemove)
                    removedRows += con.Execute("Delete From Field Where Name = @Name", new { field.Name });
            }
            catch (SqlException e)
            {
                log.LogError($"updating Fields in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
