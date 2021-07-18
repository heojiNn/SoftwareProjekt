using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XCV.Data
{
    /// <summary>
    /// This class is used and instanciated for creating, updating, deleting or accessing values in context to
    /// the generation of worddocuments for an offer.
    /// </summary>
    public class DocumentConfig
    {

        public int offerId { get; set; } // Id of the parent offer.

        [Required(ErrorMessage = "Einer Dokumentenkonfiguration muss ein Namen gegeben werden"),
        MaxLength(30, ErrorMessage = "Die Länge des Namens ist auf 30 Zeichen beschränkt")]
        public string Name { get; set; } = "";
        public IList<EmployeeConfig> employeeConfigs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
