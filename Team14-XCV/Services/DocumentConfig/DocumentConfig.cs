using System.Collections.Generic;

namespace XCV.Services.DocumentConfig
{
    // Inputparameter of Documentgenerator.
    public class DocumentConfig
    {
        // Id of the parent offer.
        private int offerId { get; set; }


        public DocumentConfig()
        {
            //TODO: initialize and manage employeeConfigs:

            // On Creation (on the offer page) this instance adds n EmployeeConfig to employeeConfigs (for each employee in the offer).
            // Whenever employees are added or removed from the offer, their EmployeeConfig is to be added/removed from employeeConfigs-collection (of each DocumentConfig)
            // This could happen automatically by linking the Configs to the list of offerEmployees (more difficult)
            // can also be done manually by having the user to edit the config once he removed/added employees from the offer (invalid Config)
        }

        // Identifiying description, unique
        // When creating a config this should be set according to a user input.
        private string description { get; set; }

        /// <summary>
        /// Collection of individual Employee Configurations. <para> </para>
        /// Usecase: In the Documentgeneration you add the content of each employeeConfig to be displayed in the Worddocument iteratively.
        /// </summary>
        public IEnumerable<EmployeeConfig> employeeConfigs { get; set; }
    }

}
