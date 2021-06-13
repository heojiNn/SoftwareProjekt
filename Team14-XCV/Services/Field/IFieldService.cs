using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface IFieldService
    {
        // Summary:
        //     GetAllFields from Persistece.
        // Returns:
        //     A List of Fields that might be  empty
        //
        // Exceptions:
        //   Exception:
        //     Could not reach Persistence: {subPath}/{fileName}
        public IEnumerable<Field> GetAllFields();


        // Summary:
        //     UpdateAllFields in Persistece.
        // Parameters:
        //   fields:
        //     The fields to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Field updated  Persitence  {fileName}
        public void UpdateAllFields(IEnumerable<Field> fields);
    }
}
