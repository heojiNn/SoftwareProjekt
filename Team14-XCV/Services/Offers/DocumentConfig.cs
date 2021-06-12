using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team14.Services.Offers
{

    public class DocumentConfig 
    {
        // JEDER Vertriebler hat EINE Config fuer JEDES Angebot fuer JEDEN Mitarbeiter -> Zuweisung dieser Instanz an Vertriebler
        IEnumerable<EmployeeConfig> employeeConfigs { get; set; }

        // Es fehlt die Menge der Angebote und darauf aufbauend eine Menge von DocumentConfigs, die jedem Benutzter mit "Sales"-Rolle zustehen.
        // Eine Config sollte idealerweise alle Teilattributauswahlen beeinhalten koennen: Beispiel : showHardskill -> C#, usw.

       




    }

}
