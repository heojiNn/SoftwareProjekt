using System;
using System.Collections.Generic;

namespace Team14.Data
{
    public class ChangeResult : EventArgs
    {
        public IEnumerable<string> InfoMessages { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccesMessage { get; set; }
    }


    public class NoResult : EventArgs
    {
        public string Message { get; set; }
    }











    public class ValidationResult : EventArgs
    {
        public string ErrorMessage { get; set; }
    }

}
