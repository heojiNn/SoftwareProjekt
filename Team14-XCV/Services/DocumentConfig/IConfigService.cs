
using System;
using System.Collections.Generic;

namespace XCV.Data
{
    public interface IConfigService
    {
        ///=======================================================
        ///============= Methods for DocumentConfigs =============
        ///=======================================================

        /// General ideas:

        /// States:
        /// User must have at least one active DocumentConfig in order to generate a Document or enter the OfferEmployeeConfigEdit page.
        /// User with at least one DocumentConfig must have a DocumentConfig selected in order to download/edit it aswell.
        /// Upon entering the OfferPage, a "favorite"/active, previously selected DocumentConfig should be selected

        /// Functions:
        /// User can selected/deselect/delete/create/download a configuration for each single offer (many configs may exists parallel in one offer)
        /// select: activates this configuration to be "active" (for generating the document); implies "deselect" for the prior config
        /// deselect: deactivate the prior config -> in this state no config is selected
        /// delete: removes an item from the "DocumentConfigurationList"
        /// create: creates a new DocumentConfig instance within "DocumentConfigurationList"

        ///=======================================================

        public event EventHandler<ChangeResult> ChangeEventHandel;

        /// <summary>
        /// Validates wheather or not update can be made to a document/employeeconfig
        /// </summary>
        /// <param name="o"></param>
        /// <param name="newVersion"></param>
        /// <param name="opt"></param>
        public void ValidateUpdate(Offer o, DocumentConfig newVersion, EmployeeConfig opt);
        /// <summary>
        /// Validates wheather or not a DocumentConfig can be created within a specific offer
        /// </summary>
        /// <param name="o"></param>
        /// <param name="newVersion"></param>
        public void ValidateCreate(Offer o, DocumentConfig newVersion);

        /// <summary>
        /// If an offer has an active selection it will be returned by this method, else null
        /// </summary>
        /// <param name="o"></param>
        /// <returns>DocumentConfig, null</returns>
        public DocumentConfig GetSelectedConfig(Offer o);
        /// <summary>
        /// Deletes the previous selected/active config and replaces it with cfg within offer o
        /// </summary>
        /// <param name="o"></param>
        /// <param name="cfg"></param>
        public void SaveSelectedConfig(Offer o, DocumentConfig cfg);
        /// <summary>
        /// Deletes the current selected Config
        /// </summary>
        /// <param name="o"></param>
        /// <param name="cfg"></param>
        public void DeleteSelectedConfig(Offer o, DocumentConfig cfg);
        /// <summary>
        /// Returns a document config with name 'name' in Offer o. <para></para>
        /// Does this by calling GetAllDocumentConfigs and filtering for 'name'
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <returns>DocumentConfig</returns>
        public DocumentConfig GetDocumentConfig(Offer o, string name);
        /// <summary>
        /// Returns all DocumentConfigs of the specified offer o
        /// </summary>
        /// <param name="o"></param>
        /// <returns>Collection of DocumentConfigs</returns>
        public IEnumerable<DocumentConfig> GetAllDocumentConfigs(Offer o);
        /// <summary>
        /// Adds all the data to the config - State: After Initialization
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns>The created DocumentConfig</returns>
        public DocumentConfig CreateDefaultDocumentConfig(Offer parent, string name);
        /// <summary>
        /// Deletes a DocumentConfig and all its childcomponent data from an Offer 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="cfg"></param>
        public void DeleteDocumentConfig(Offer o, DocumentConfig cfg);


        public void UpdateDocumentConfig(DocumentConfig cfg);

        /// <summary>
        /// Adds a new EmployeeConfig to each DocumentConfig (with all data selected)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="toAdd"></param>
        public void Add(Offer o, Employee toAdd);

        /// <summary>
        /// Removes a EmployeeConfig from each DocumentConfig. <para></para>
        /// If a config contains 0 EmployeeConfigs it will be removed automatically.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="toRemove"></param>
        public void Remove(Offer o, Employee toRemove);

        ///=======================================================
        ///============= Methods for EmployeeConfigs =============
        ///=======================================================

        /// Access:
        /// On the Offer page access should've been recognised and a DocumentConfig selected for the offer with the current offer id.
        /// If User has no DocumentConfig created/selected he must add/select one on the Offer page

        /// Precondition:
        /// A DocumentConfig has to be created and selected to edit a EmployeeConfig

        /// <summary>
        /// Updates the EmployeeConfiguration
        /// </summary>
        /// <param name="o"></param>
        /// <param name="c"></param>
        /// <param name="persnr"></param>
        /// <param name="cfg"></param>
        public void UpdateEmployeeConfig(Offer o, DocumentConfig c, string persnr, EmployeeConfig cfg);
    }
}
