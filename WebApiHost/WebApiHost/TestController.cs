using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiHost
{
    public class TestController : ApiController
    {
        [Route("test")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var cp = (ClaimsPrincipal)User;
            var claims = cp.Claims.Select(x => x.Type + ":" + x.Value);

            return Json(new
            {
                status = "User is: " +
                    (User.Identity.IsAuthenticated ? "authenticated" : "anonymous"),
                claims = claims.ToArray()
            });
        }
    }
}
