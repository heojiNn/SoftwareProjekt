using System;
using System.Collections.Generic;

namespace Team14
{
    public class Employee
    {
        private string _vorname;
        private string _nachname;
        private List<string> _projekte;
        private DateTime _geburtstag;

        public Employee(string vorname, string nachname, List<String> projekte, DateTime geburtstag)
        {
            _vorname = vorname;
            _nachname = nachname;
            _projekte = projekte;
            _geburtstag = geburtstag;
        }

        public string Vorname
        {
            get { return _vorname; }
            set { _vorname = value; }
        }

        public string Nachname
        {
            get { return _nachname; }
            set { _nachname = value; }
        }

        public List<String> Projekte
        {
            get { return _projekte; }
            set { _projekte = value; }
        }

        public DateTime Geburtstag
        {
            get { return _geburtstag; }
            set { _geburtstag = value; }
        }

    }
}
