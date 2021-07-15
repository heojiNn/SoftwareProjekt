using System;
using System.Collections.Generic;


namespace XCV.Data
{
    public interface IProjectService
    {

        /// <summary>
        /// Returns all Projects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> ShowAllProjects();

        /// <summary>
        /// Returns one Project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project ShowProject(int id);

        //CRUD:


        /// <summary>
        /// Creates a new Project with a title
        /// </summary>
        /// <param name="title"></param>
        public void Create(string title);
        /// <summary>
        /// Updates an existing Project by Reference to it's Id
        /// </summary>
        /// <param name="newP"></param>
        public void Update(Project newP);
        /// <summary>
        /// Deletes an existing Project by Reference to it's Id
        /// </summary>
        /// <param name="toDelete"></param>
        public void Delete(Project toDelete);


        /// <summary>
        /// Adds an activity to am existing project
        /// </summary>
        /// <param name="p"></param>
        /// <param name="activity"></param>
        public void Add(Project p, string activity);
        /// <summary>
        /// Removes an activity to an existing project
        /// </summary>
        /// <param name="p"></param>
        /// <param name="activity"></param>
        public void Remove(Project p, string activity);
        /// <summary>
        /// Adds an Employee to an existing Project without specified activities
        /// </summary>
        /// <param name="p"></param>
        /// <param name="doneBy"></param>
        /// <param name="activity"></param>
        public void Add(Project p, Employee doneBy, string activity = "ohne sepz. Aktivität");
        /// <summary>
        /// Adds an Employee to an existing Project without specified activities 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="doneBy"></param>
        /// <param name="activity"></param>
        public void Remove(Project p, Employee doneBy, string activity = "ohne sepz. Aktivität");



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
