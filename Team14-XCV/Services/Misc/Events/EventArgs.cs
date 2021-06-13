using System;
using System.Collections.Generic;

namespace Team14.Data
{
    public class ChangeResult : EventArgs
    {
        public IEnumerable<string> InfoMessages { get; set; } = new List<string>();
        public IEnumerable<string> ErrorMessages { get; set; } = new List<string>();
        public string SuccesMessage { get; set; } = "";
    }


    public class NoResult : EventArgs
    {
        public string Message { get; set; } = "";
    }







    public class LoginResult : EventArgs
    {
        public string ErrorMessage { get; set; } = "";
    }

}
