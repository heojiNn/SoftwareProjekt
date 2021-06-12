using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Team14.Data
{
    public class CustomAuthentiProvider : AuthenticationStateProvider
    {
        private readonly IEnumerable<Employee> employees;
        private readonly IJSRuntime _jSRuntime;
        // private readonly IJSInProcessRuntime _jSInProcessRuntime;

        //
        // is injectedt with a js-runtime
        //
        public CustomAuthentiProvider(IJSRuntime jSRuntime, IEmployeeService EmployeeServiceCache)
        {
            _jSRuntime = jSRuntime;
            employees = EmployeeServiceCache.Employees;

        }


        // this gets called at first load
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            string serialisedData = null;

            try
            {
                serialisedData = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentEmployee");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"  to early");
            }
            if (serialisedData != null)
            {
                Employee Employee = JsonSerializer.Deserialize<Employee>(serialisedData, options);
                Console.WriteLine($"  deserialize Name {Employee.PersoNumber}");
                if (Employee != null)
                    identity = SetupClaimsForEmployee(Employee);
            }
            else Console.WriteLine("");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        private static ClaimsIdentity SetupClaimsForEmployee(Employee Employee)
        {
            ClaimsIdentity identity;
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, Employee.PersoNumber)
            };

            foreach (AccessRole role in Employee.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString().ToLower() ));
            }
            identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }


        // function will tell the app that Employee is authenticated and save the state in 
        // the session store if authenticated through the _comm.
        public void Login(string name, string password)
        {
            Employee employee;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Es muss etwas eingegeben werden!");
            try
            {
                employee = employees.First(u => u.PersoNumber.Equals(name) && u.Password.Equals(password));
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Kombination aus Employee und Passwort existiert nicht!");
            }
            try
            {
                var identity = SetupClaimsForEmployee(employee);

                Console.WriteLine("Creating Cookie...");
                string serialisedData = JsonSerializer.Serialize(employee, options);

                var tsk =  _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentEmployee", serialisedData);

                // tell the application that Employee state has been changed

                var claimsPrincipal = new ClaimsPrincipal(identity);
                var authenticationState = new AuthenticationState(claimsPrincipal);
                NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

                Console.WriteLine("Login erfolgreich!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Validation Error {e}");
            }
        }

        public void Logout()
        {
            var tsk = _jSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentEmployee");
            var empty = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(empty)));

            Console.WriteLine("Logout erfolgreich!");
        }


        private readonly JsonSerializerOptions options = new()
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = false
        };
    }
}
