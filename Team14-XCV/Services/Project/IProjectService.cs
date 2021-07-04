using System;
using System.Collections.Generic;


namespace XCV.Data
{
    public interface IProjectService
    {

        // Summary:
        //     all stored pros
        public IEnumerable<Project> ShowAllProjects();

        // Summary:
        //     one pro
        public Project ShowProject(int id);




        //
        public void Create(string title);
        public void Update(Project newP);
        public void Delete(Project toDelete);



        public void Add(Project p, string activity);
        public void Remove(Project p, string activity);

        public void Add(Project p, Employee doneBy, string activity = "");
        public void Remove(Project p, Employee doneBy, string activity = "");



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
