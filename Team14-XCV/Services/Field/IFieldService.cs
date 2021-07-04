using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface IFieldService
    {
        /// <summary>
        ///         Validates against DataAnotations on the  Field-Entity
        /// </summary>
        ///
        /// <returns>
        ///          ErrorMessages
        /// </returns>
        public IEnumerable<string> ValidateFields(IEnumerable<Field> fields);


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
        ///         UpdateAllFields in persistence.
        /// </summary>
        /// <remarks>
        ///         ueses private_ValidateField(field),
        ///         Catches SqlExceptions and logs them
        /// </remarks>
        ///
        /// <param name="fields">
        ///         The fields to replace the old ones.
        /// </param>
        public (int added, int removed) UpdateAllFields(IEnumerable<Field> fields);


        /// <summary>
        ///         Adds one Field to persistence.
        /// </summary>
        ///
        /// <event cref="OnChange">
        ///     Succes: {newField}: wurde hinzugefügt.
        /// or
        ///     Error:  {newField}: kann nicht hinzugefügt werden, da es schon enthalten ist.
        ///     Error:  Brachen benötigen einen Namen / darf 50 Zeichen {newField}:
        ///</event>
        public void CreateField(Field newField);
        /// <summary>
        ///         Removes one Field to persistence.
        /// </summary>
        ///
        /// <event cref="OnChange">
        ///     Succes: {newField}: wurde entfernt.
        /// or
        ///     Error:  {newField}: kann nicht entfernt werden, da es nicht enthalten ist.
        ///</event>
        public void RemoveField(Field field);



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
