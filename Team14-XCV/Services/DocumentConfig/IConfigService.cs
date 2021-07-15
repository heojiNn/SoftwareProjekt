
using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface IConfigService
    {

        // General Interaction
        // Possible Methods:
        // selected/deselect/delete/create/update//clear/undo/redo/reset/copy/rename/download

        // Document of Employee Detailview: Possible Methods:
        // selected/deselect/delete/create/update//clear/undo/redo/reset/copy/rename/download


        //=======================================================
        //============= Methods for DocumentConfigs =============
        //=======================================================

        // General ideas and possible additions:

        // States:
        // User must have at least one DocumentConfig in order to generate a Document(else no Input can be processed) [Alternative: Define a copy of the generate Function that prints the defaults{all Employees with all data}] or to enter the OfferEmployeeConfigEdit page.
        // User with at least one DocumentConfig must have a DocumentConfig selected in order to download/edit it aswell.
        // Upon entering the OfferPage, the latest selected DocumentConfig should be preselected [Alternative: Feature for setting a "favorite" Config for an offer which is preselected instead.]

        // Functions:
        // User can selected/deselect/delete/create/copy/rename a configuration
        // select: activates this configuration to be "active" (for generating the document); implies "deselect" for the prior config
        // deselect: deactivate the prior config -> in this state no config is selected
        // delete: removes an item from the "DocumentConfigurationList"
        // create: creates a new DocumentConfig instance within "DocumentConfigurationList"
        // copy: copies a certain DocumentConfig with a default description of "<name> (1)" or "<name> (2)", as long as the previous copynumber doesn't exist/ is unique
        // rename: change of the description of DocumentConfig, must be unique

        //=======================================================

        public event EventHandler<ChangeResult> ChangeEventHandel;

        public void ValidateUpdate(Offer o, DocumentConfig newVersion, EmployeeConfig opt);

        public void ValidateCreate(Offer o, DocumentConfig newVersion);

        /// <summary>
        /// If an offer has an active selection it will be returned by this method, else null
        /// </summary>
        /// <param name="o"></param>
        /// <returns>DocumentConfig, null</returns>
        public DocumentConfig GetSelectedConfig(Offer o);

        public void SaveSelectedConfig(Offer o, DocumentConfig cfg);

        public void DeleteSelectedConfig(Offer o, DocumentConfig cfg);

        public DocumentConfig GetDocumentConfig(Offer o, string name);


        public IEnumerable<DocumentConfig> GetAllDocumentConfigs(Offer o);

        /// <summary>
        /// Adds all the data to the config - State: After Initialization
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public DocumentConfig CreateDefaultDocumentConfig(Offer parent, string name);

        public void DeleteDocumentConfig(Offer parent, DocumentConfig cfg);


        public void UpdateDocumentConfig(DocumentConfig cfg);


        //=======================================================
        //============= Methods for EmployeeConfigs =============
        //=======================================================

        // Dokumentenkonfiguration - EmployeeConfig: TODO:


        // Access:
        // On the Offer page access should've been recognised and a DocumentConfig selected for the offer with the current offer id.
        // If User has no DocumentConfig created/selected he must add/select one on the Offer page

        // Precondition:
        // So we are within the correct DocumentConfig and the path over the EmployeeDetailView should give the information about which employee we selected.
        // The selected Employee needs to be linked to an EmployeeConfig within the DocumentConfig.

        // Functions:
        // selection-methods
        //  buttons: cancel, confirm
        // (optional) buttons: clear/unfill, default/fill, redo (restore), undo (discard)


        public void UpdateEmployeeConfig(Offer o, DocumentConfig c, string persnr, EmployeeConfig cfg);
    }
}
