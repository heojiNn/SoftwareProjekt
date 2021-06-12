using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface IFieldService
    {
        public IEnumerable<Field> GetAllFields();


        public void UpdateField(IEnumerable<Field> skills);

        public event EventHandler<NoResult> SearchEventHandel;
    }

}
