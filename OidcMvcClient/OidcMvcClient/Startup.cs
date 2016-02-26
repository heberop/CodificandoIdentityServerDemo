using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OidcMvcClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "cookies",
            });

            // TODO: configure trust to the token service
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "oidc",
                SignInAsAuthenticationType = "cookies",
                Authority = "https://localhost:44333",
                ClientId = "Acme.WebApp",
                RedirectUri = "https://localhost:44300",
                ResponseType = "id_token token",
                Scope = "openid email profile acmescope api1",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var user = n.AuthenticationTicket.Identity;
                        user.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
