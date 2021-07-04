using System.Collections.Generic;

namespace XCV.Data
{
    // Inputparameter of Documentgenerator.
    public class DocumentConfig
    {
        //===========================================================================================================================================//

        // Dokumentenkonfiguration: TODO: (needs: -)

        // Access:
        // Use access role to identify user. -> Access role returns employee-instance.
        // Employee instance contains: "DocumentConfigurationList".
        // Display the Configurations in a HTML element on the offer with the latest chosen Config being preselected.
        // How to implement latest chosen Config? Persist in Employee as attribute?

        // Functions:
        // User can selected/deselect/delete/create/copy/rename a configuration
        // select: activates this configuration to be "active" (for generating the document); implies "deselect" for the prior config
        // deselect: deactivate the prior config -> in this state no config is selected
        // delete: removes an item from the "DocumentConfigurationList"
        // create: creates a new DocumentConfig instance within "DocumentConfigurationList"
        // copy: copies a certain DocumentConfig with a default description of "<name> (1)" or "<name> (2)", as long as the previous copynumber doesn't exist/ is unique
        // rename: change of the description of DocumentConfig, must be unique

        // States:
        // User must have at least one DocumentConfig in order to generate a Document(else no Input can be processed) [Alternative: Define a copy of the generate Function that prints the defaults{all Employees with all data}] or to enter the OfferEmployeeConfigEdit page.
        // User with at least one DocumentConfig must have a DocumentConfig selected in order to download/edit it aswell.
        // Upon entering the OfferPage, the latest selected DocumentConfig should be preselected [Alternative: Feature for setting a "favorite" Config for an offer which is preselected instead.]


        //===========================================================================================================================================//

        // Dokumentengeneration: TODO: (needs: Dokumentenkonfiguration)

        // Parameter:
        // DocumentConfig config, contains all the EmployeeConfigs of the document which are to be iterated through individually and displayed in the document.

        // Content:
        // Good looking document structure for EmployeeConfigs
        // "Good looking" := Information-display of individual Employees shoudln't bn displayed cross-page.


        private int offerId { get; set; } // Id of the parent offer.
        private string PersoNumber { get; set; } // PersNr of the parent employee.
        private string description { get; set; } // Identifiying description, unique
        public IEnumerable<EmployeeConfig> employeeConfigs { get; set; }




        // by mario    ------------------- private OfferService oS;
        // public DocumentConfig()
        // {   // Initializes with defaults.
        //     foreach (Employee e in oS.ShowOffer(offerId).participants)
        //     {
        //         new EmployeeConfig(e);
        //     }
        // }














        // Whenever employees are added or removed from the offer, their EmployeeConfig is to be added/removed from employeeConfigs-collection (of each DocumentConfig)

        // This could happen automatically by linking the Configs to the list of offerEmployees (more difficult)
        // can also be done manually by having the user to edit the config once he removed/added employees from the offer (invalid Config)
    }
}
