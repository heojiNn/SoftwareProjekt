using System;

namespace Team14.Data
{
    public class Employee
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public List<string> Projekte { get; set; }
        public DateTime Birthday { get; set; }
    }
}
