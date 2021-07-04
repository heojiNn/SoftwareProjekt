using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace XCV.Data
{
    interface IOfferService
    {
        //--------------------------------------------------------------//
        // Read:

        /// <summary>
        /// Returns a Collection which contains the existing Offer(s) in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Offer> ShowAllOffers();

        /// <summary>
        /// Returns an existing Offer with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Offer ShowOffer(int id);

        //--------------------------------------------------------------//
        // Write:

        /// <summary>
        ///  Creates a new Offer with a unique Id.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public void Create(string title, string description);
      
        public void Create(string title, string description, Skill skill, Field field, ISet<Employee> participants); // ref. "CreateOffer"
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="o"></param>
        public void Update(Offer o);
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="o"></param>
        public void Delete(Offer o);
        /// <summary>
        /// Adds an Employee to an existing Offer o.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="offerEmp"></param>
        public void Add(Offer o, Employee offerEmp);
        /// <summary>
        /// Adds a Skill to an existing Offer o.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public void Add(Offer o, Skill s);
        /// <summary>
        /// Adds a Field to an existing offer o.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="f"></param>
        public void Add(Offer o, Field f);
        /// <summary>
        /// Removes an Employee from an existing Offer o.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="offerEmp"></param>
        public void Remove(Offer o, Employee offerEmp);
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="o"></param>
        /// <param name="s"></param>
        public void Remove(Offer o, Skill s);
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="o"></param>
        /// <param name="f"></param>
        public void Remove(Offer o, Field f);
        /// <summary>
        /// Creates a new Offer which is identical to Offer o.
        /// </summary>
        /// <param name="o"></param>
        public void Copy(Offer o);

        //--------------------------------------------------------------//
        // Business:

        /// <summary>
        /// Processes Events.
        /// </summary>
        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
