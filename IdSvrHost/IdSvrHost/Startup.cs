using System.Linq;
using IdentityModel;
using IdentityServer3.Core.Configuration;
using Owin;
using Serilog;
using IdentityServer3.Core.Services.InMemory;
using IdentityServer3.Core.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using System.Security.Cryptography.X509Certificates;

namespace IdSvrHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var cert = new X509Certificate2("idsrv3test.pfx", "idsrv3test");
            var factory = new IdentityServerServiceFactory();

            //usuarios
            var users = new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "2342342545",
                    Username = "patolino",
                    Password = "patolino",
                    Claims = new List<Claim>
                    {
                        new Claim("email", "patolino@acme.com"),
                        new Claim("name", "Patolino"),
                        new Claim("role", "user"),
                        new Claim("role", "manager"),
                        new Claim("http://acme.com/claims/customclaim", "Custom Value")
                    }
                }
            };

            //clients
            var clients = new List<Client>
            {
                new Client
                {
                    ClientId = "Acme.WebApp",
                    ClientName = "Acme Web App",
                    RedirectUris = {"https://localhost:44300"},
                    AllowedScopes = {"openid", "email",
                                    "profile", "acmescope",
                                    "api1"},
                    Flow = Flows.Implicit
                }
            };

            //scopes
            var scopes = new List<Scope>
            {
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.OpenId,
                new Scope
                {
                    Name = "acmescope",
                    DisplayName = "Acme Info",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("http://acme.com/claims/customclaim")
                    }
                },
                new Scope
                {
                    Name = "api1",
                    DisplayName = "Acme API",
                    Type = ScopeType.Resource
                }
            };

            factory.UseInMemoryUsers(users);
            factory.UseInMemoryClients(clients);
            factory.UseInMemoryScopes(scopes);

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Acme Corp",
                SigningCertificate = cert,
                Factory = factory
            });
        }
    }
}