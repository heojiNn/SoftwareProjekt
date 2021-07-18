using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface IFieldService
    {
        /// <summary>
        ///         GetAllFields from persistence.
        /// </summary>
        /// <remarks>
        ///         Catches all SqlExceptions, logs an Error: {e.Message}
        ///         and will just return an empty collection
        /// </remarks>
        ///
        /// <returns>
        ///          A collection of Fields, that might be empty
        /// </returns>
        public IEnumerable<Field> GetAllFields();



        /// <summary>
        ///         Adds one Field to persistence.
        /// </summary>
        /// <remarks>
        ///         Catches SqlExceptions and just logs them
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///         Succes: {toAdd}, wurde zur Liste der Brachen hinzugefügt.
        /// or
        ///static   Error:  Name.Length war nicht (&lt50 && &lt1) {newField}
        ///dyn      Error:  {toAdd}, kann nicht hinzugefügt werden, da es schon enthalten ist.
        /// </event>
        public int CreateField(Field toAdd, bool justValidate = false);
        /// <summary>
        ///         Removes one Field to persistence.
        /// </summary>
        /// <remarks>
        ///         Catches SqlExceptions and just logs them
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///         Succes: {toRemove}, wurde aus Liste der Brachen entfernt.
        /// or
        ///static   Error:  {toRemove}, kann nicht entfernt werden, da es nicht enthalten ist.
        /// </event>
        public int DeleteField(Field toRemove);

        /// <summary>
        ///         UpdateAllFields in persistence.
        /// </summary>
        /// <remarks>
        ///         checks the Difference
        ///         for the actual change on persitence  ueses Create/DeleteField(field)
        /// </remarks>
        ///
        /// <param name="fields">
        ///         The fields to replace the old ones.
        /// </param>
        public (int added, int removed) UpdateAllFields(IEnumerable<Field> fields, bool justValidate = false);



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
