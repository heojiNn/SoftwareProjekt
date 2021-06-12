using System;
using System.Collections.Generic;


namespace Team14.Data
{
    public class EmployeeServiceCache : IEmployeeService
    {
        public IEnumerable<Employee> Employees { get; } =
        new[]{
            new Employee
            {
                PersoNumber = "arnold",
                Password = "arnold",
                Roles = new AccessRole[] {AccessRole.Sales, AccessRole.Admin}
            },
            new Employee  {
                PersoNumber = "susane",
                Password = "susane",
                Roles =  new AccessRole[] {AccessRole.Sales}
            },
            new Employee {
                PersoNumber = "00-00",
                Password = "00-00",
                Roles =  Array.Empty<AccessRole>()
            },
            new Employee {
                PersoNumber = "00-01",
                Password = "00-01",
                Roles = new AccessRole[] {AccessRole.Admin}
            },
            new Employee {
                PersoNumber = "00-02",
                Password = "00-02",
                Roles = new AccessRole[] {AccessRole.Sales}
            },
            new Employee {
                PersoNumber = "00-03",
                Password = "00-03",
                Roles = new AccessRole[] {AccessRole.SuperAdmin}
            }
        };

    }
}