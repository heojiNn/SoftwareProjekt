using System;
using System.Collections.Generic;

public class Employee
{
    public string Vorname { get; set; }

    public string Nachname { get; set; }

    public List<string> Projekte { get; set; }

    public DateTime Geburtstag { get; set; }


    public override string ToString()
    {
        return "Vorname: " + Vorname + "\nNachname: " + Nachname + "\nProjekte: " + String.Join(", ", Projekte.ToArray()) + "\nGeburstag: " + Geburtstag.ToString("dd.MM.yyyy");
    }
}



