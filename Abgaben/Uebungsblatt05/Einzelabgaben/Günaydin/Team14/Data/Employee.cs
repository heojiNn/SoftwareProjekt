using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team14.Data
{
    public class Employee
    {
        public String Vorname { get; set; }
        public String Nachname { get; set; }
        public DateTime Geburtstag { get; set; }
        public List <String> Projekte{ get; set; }
    }
}
