using System;
using System.Collections.Generic;


namespace Team14.Data
{
    public class ProjectService
    {
        private readonly List<Project> theSigelton;
        public ProjectService()
        {
            theSigelton = new()
            {

                new Project() { Title = "p1", Description = "p1" },
                new Project() { Title = "p2", Description = "p2" },
                new Project() { Title = "p3", Description = "sdfsdf" }
            };

        }


        public List<Project> GetSigelton()
        {
            return theSigelton;
        }

    }
}
