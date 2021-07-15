using System;
using System.Collections.Generic;


namespace XCV.Data
{
    public interface IProjectService
    {

        /// <summary>
        ///     Returns a Collection which contains all existing Project(s) in the database.
        /// </summary>
        /// <returns>
        ///     A Collection of Projects or null.
        /// </returns>
        public IEnumerable<Project> ShowAllProjects();

        /// <summary>
        ///     Returns an existing Project with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     A project or null.
        /// </returns>
        public Project ShowProject(int id);

        //CRUD:



        /// <summary>
        ///     Creates a project with a unique id.
        /// </summary>
        /// <param name="title"></param>
        public void Create(string title);
        /// <summary>
        ///     Creates a project with a unique id.
        /// </summary>
        /// <remarks>
        ///     uses Validate(Project p)
        ///     <para> Error: through the method constraints </para>
        ///     <para> Error: through the DataAnnotaion </para>
        /// </remarks>
        /// <param name="p"></param>
        /// <event cref="OnChange">
        ///         <para>Error:  Der Projektanfang kann nicht vor dem Jahr 2011 liegen. </para>
        ///         <para>Error:  Das Enddatum muss hinter dem Beginn liegen. </para>
        ///         <para>Error:  Das Projekt muss vor dem Jahr 2100 beendet werden. </para>
        ///         <para>Error:  Die Beschreibung des Zwecks ist zu kurz. (length < 2)</para>
        ///         <para>Error:  Die Beschreibung des Zwecks ist zu lang. (length > 200) </para>
        ///         <para>Error:  Die Beschreibung der Tätigkeit ist zu kurz. (length < 2) </para>
        ///         <para>Error:  Die Beschreibung der Tätigkeit:({key}) ist zu lang. (length > 50) </para>
        ///         <para>Error:  Der Titel darf nicht leer sein. </para>
        ///         <para>Error:  Es existiert schon ein Projekt mit diesem Titel. </para>
        ///         <para>Error:  Der Titel darf nicht länger als 20 Zeichen sein. </para>
        ///         <para>Error:  Die Beschreibung darf nicht länger als 1000 Zeichen sein. </para>
        ///         <para>Error:  Es trat ein Fehler in der Persistenz auf {e.Message}. </para>     
        ///
        ///   or    <para>Success: Das Projekt:{p.Title} wurde mit der Id: {newId} erstellt.</para>
        /// </event>
        public void Create(Project p);
        /// <summary>
        ///     Checks whether <paramref name="newVersion"/> meets all the constraints. 
        /// </summary>
        /// <remarks>
        ///     <para> Informs via a ChangeEvent about:</para>
        ///     <para> Info: what will be changed </para>
        ///     <para> Error: through the method constraints </para>
        ///     <para> Error: through the DataAnnotaion </para>
        /// </remarks>
        /// <param name="newVersion"></param>
        /// <event cref="OnChange">
        ///         <para> Info: Der Projekttitel wird geändert. </para>
        ///         <para> Info: Die Beschreibung wird geändert. </para>
        ///         <para> Info: Die Laufzeit wird geändert. </para>
        ///         <para> Info: Die Branche wird geändert. </para>
        ///         <para> Info: Die Projektzwecke werden geändert. </para>
        ///         <para> Info: Die Projekttätigkeiten und/oder zugehörige MitarbeiterInnen bzw. Skills werden geändert. </para>
        ///   or
        ///         <para>Error:  Der Projektanfang kann nicht vor dem Jahr 2011 liegen. </para>
        ///         <para>Error:  Das Enddatum muss hinter dem Beginn liegen. </para>
        ///         <para>Error:  Das Projekt muss vor dem Jahr 2100 beendet werden. </para>
        ///         <para>Error:  Die Beschreibung des Zwecks ist zu kurz. (length < 2)</para>
        ///         <para>Error:  Die Beschreibung des Zwecks ist zu lang. (length > 200) </para>
        ///         <para>Error:  Die Beschreibung der Tätigkeit ist zu kurz. (length < 2) </para>
        ///         <para>Error:  Die Beschreibung der Tätigkeit:({key}) ist zu lang. (length > 50) </para>
        ///         <para>Error:  Der Titel darf nicht leer sein. </para>
        ///         <para>Error:  Es existiert schon ein Projekt mit diesem Titel. </para>
        ///         <para>Error:  Der Titel darf nicht länger als 20 Zeichen sein. </para>
        ///         <para>Error:  Die Beschreibung darf nicht länger als 1000 Zeichen sein. </para>
        ///         <para>Error:  Es trat ein Fehler in der Persistenz auf {e.Message}. </para>     
        ///  or
        ///         <para>Success: Das Projekt:{p.Title} wurde mit der Id: {newId} erstellt.</para>
        /// </event>
        public void ValidateUpdate(Project newVersion);

        /// <summary>
        ///     Updates the passed Project.
        /// </summary>
        /// <remarks>
        ///     uses ValidateUpdate(Project p)
        /// </remarks>
        /// <param name="newP"></param>
        public void Update(Project newP);

        /// <summary>
        ///     Deletes the passed project and all its references. 
        /// </summary>
        /// <param name="toDelete"></param>
        public void Delete(Project toDelete);

        /// <summary>
        /// Adds an activity to a project.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="activity"></param>
        public void Add(Project p, string activity);
        /// <summary>
        /// Removes an activity from a project.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="activity"></param>
        public void Remove(Project p, string activity);
        /// <summary>
        /// Adds an employee to a project activity.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="doneBy"></param>
        /// <param name="activity"></param>
        public void Add(Project p, Employee doneBy, string activity = "ohne Tätigkeit");
        /// <summary>
        /// Removes an employee from a project activity.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="doneBy"></param>
        /// <param name="activity"></param>
        public void Remove(Project p, Employee doneBy, string activity = "ohne Tätigkeit");



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
