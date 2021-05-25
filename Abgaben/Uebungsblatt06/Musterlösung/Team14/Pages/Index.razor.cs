using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Team14.Pages
{
    public partial class Index
    {
        //employee-instances
        List<Employee> Mitarbeiter = new List<Employee> {
            new Employee{
                Vorname = "Moritz",
                Nachname = "Boche",
                Geburtstag = new DateTime(1995, 02, 02),
                Projekte = new List<string>{ "Xitaso", "Projekt B"}
            },
             new Employee{
                Vorname = "Brigitte",
                Nachname = "Glogger",
                Geburtstag = new DateTime(1995, 02, 02),
                Projekte = new List<string>{ "Xitaso", "Projekt A"}
            },
            new Employee{
                Vorname = "Fabian",
                Nachname = "Günaydin",
                Geburtstag = new DateTime(1998, 04, 04),
                Projekte = new List<string>{ "Xitaso", "Projekt C", "Projekt D"}
            },
            new Employee{
                Vorname = "Jonathan",
                Nachname = "Leis",
                Geburtstag = new DateTime(1999, 01, 01),
                Projekte = new List<string>{ "Xitaso"}
            },
            new Employee{
                Vorname = "Mario",
                Nachname = "Melle",
                Geburtstag = new DateTime(1993, 09, 17),
                Projekte = new List<string>{ "Xitaso"}
            },
            new Employee{
                Vorname = "Philipp",
                Nachname = "Wagner",
                Geburtstag = new DateTime(1994, 12, 07),
                Projekte = new List<string>{ "Xitaso"}
            }
        };

        /*
        [Inject]
        IJSRuntime IJS;
        
        public async Task ExportClick()
        {

            await IJS.InvokeVoidAsync("exportHTML");
        }
        */




    }
}
