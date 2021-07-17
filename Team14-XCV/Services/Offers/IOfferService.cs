using System;
using System.Collections.Generic;


namespace XCV.Data
{
    public interface IOfferService
    {

        ///--------------------------------------------------------------//
        /// Business:

        /// <summary>
        /// Processes Events.
        /// </summary>
        public event EventHandler<ChangeResult> ChangeEventHandel;
        /// <summary>
        /// Validates the Changes made to an offer
        /// </summary>
        /// <param name="newVersion"></param>
        public void ValidateUpdate(Offer newVersion);
        /// <summary>
        /// Validates the Creation of a new offer
        /// </summary>
        /// <param name="newVersion"></param>
        public void ValidateCreate(Offer newVersion);
        /// <summary>
        /// Returns end - start in Days (int)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int Runtime(DateTime end, DateTime start);




        ///--------------------------------------------------------------//
        /// Persistence:
        /// Read:

        /// <summary>
        /// Returns a Collection which contains the existing Offer(s) in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Offer> ShowAllOffers();

        /// <summary>
        /// Returns a Collection which contains all the skills all exisitng Offer(s) have combined.
        /// </summary>
        /// <returns></returns>
        public List<Skill> ShowAllOfferSkills();

        /// <summary>
        /// Returns a Collection which contains all the fields all exisitng Offer(s) have combined.
        /// </summary>
        /// <returns></returns>
        public List<Field> ShowAllOfferFields();

        /// <summary>
        /// Returns an existing Offer with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Offer ShowOffer(int id);

        /// <summary>
        /// Returns an instance of an Employee within an existing Offer or null if he isn't within that offer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="persnr"></param>
        /// <returns>Employee, null</returns>
        public Employee ShowOfferEmployee(int id, string persnr);

        /// <summary>
        /// Returns the Id of the most recently created offer
        /// </summary>
        /// <returns></returns>
        public int GetLastId();
        /// <summary>
        /// Returns the Id of the Offer which would be created next
        /// </summary>
        /// <returns></returns>
        public int GetNextId();

        /// <summary>
        /// Reseeds the Id-Increment to 1 after deleting all Offers (OfferIds wont stack until Int-Overflow over time)
        /// </summary>
        public void ResetId();

        ///--------------------------------------------------------------//
        /// Write:

        /// <summary>
        ///  Creates a new Offer with a unique Id.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public void Create(string title, string description, DateTime start, DateTime end);
        /// <summary>
        /// Alternative to Creating a new Offer instantaneously
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="skill"></param>
        /// <param name="field"></param>
        /// <param name="participants"></param>
        public void Create(string title, string description, DateTime start, DateTime end, Skill skill, Field field, ISet<Employee> participants);
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
        /// Creates a new Offer which is identical to Offer o (apart from the Id)
        /// </summary>
        /// <param name="o"></param>
        public void Copy(Offer o);
    }
}
