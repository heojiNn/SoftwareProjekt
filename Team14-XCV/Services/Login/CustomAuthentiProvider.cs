using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace XCV.Data
{
    public class CustomAuthentiProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly ILogger<CustomAuthentiProvider> _logger;
        private readonly IAccountService _employeeService;
        private readonly string sessionStorageKey = "currentEmployee";

        public CustomAuthentiProvider(IJSRuntime jSRuntime, ILogger<CustomAuthentiProvider> logger, IAccountService employeeService)
        {
            _jSRuntime = jSRuntime;
            _logger = logger;
            _employeeService = employeeService;
        }


        public async Task<bool> Login(string name, string password)
        {
            Employee employee;
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(password))
            {
                OnValidation(new() { ErrorMessage = "Die Felder dürfen nicht leer bleiben." });
                return false;
            }
            try
            {
                employee = _employeeService.ShowAllProfiles().First(u => u.PersoNumber.Equals(name) && u.Password.Equals(password));
            }
            catch (InvalidOperationException)
            {
                OnValidation(new() { ErrorMessage = $"Kombination aus {name} und {password} ungültig." });
                return false;
            }

            await _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", sessionStorageKey, JsonSerializer.Serialize(employee.PersoNumber));  //set
            _logger.LogInformation($"\"{employee.PersoNumber}\" \t stores his login-data in SessionStorage");
            return true;
        }

        public async Task Logout()
        {
            var authstate = await GetAuthenticationStateAsync();
            await _jSRuntime.InvokeVoidAsync("sessionStorage.removeItem", sessionStorageKey);                                 //remove
            _logger.LogInformation($"\"{authstate.User.Identity.Name}\" \t Loged out  and data removed from SessionStorage");
        }




        // this gets called by ASP.net  on load   it uses sessionStorage
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try            // ASP is the first few tiems to  fast   but will retry automaticlly
            {
                var sessionData = await _jSRuntime.InvokeAsync<string>("sessionStorage.getItem", sessionStorageKey);           //get
                if (sessionData != null)
                    identity = SetupClaimsForEmployee(JsonSerializer.Deserialize<string>(sessionData));
            }
            catch (InvalidOperationException) { }

            var claimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        private ClaimsIdentity SetupClaimsForEmployee(string employeeNr)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, employeeNr) };
            claims.AddRange(_employeeService.ShowAllProfiles().First(u => u.PersoNumber == employeeNr).AcRoles.Select(x => new Claim(ClaimTypes.Role, x.ToString().ToLower())));
            return new ClaimsIdentity(claims, "apiauth_type");
        }



        public event EventHandler<LoginResult> ValidationEventHandel;
        protected virtual void OnValidation(LoginResult e)
        {
            EventHandler<LoginResult> handler = ValidationEventHandel;
            handler?.Invoke(this, e);
        }
    }
}
