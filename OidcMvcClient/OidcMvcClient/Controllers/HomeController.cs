using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OidcMvcClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("cookies");
            return Redirect("~/");
        }

        public async Task<ActionResult> Api()
        {
            var cp = (ClaimsPrincipal)User;
            var token = cp.FindFirst("access_token")?.Value;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("http://localhost:12688/test");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                ViewData["result"] = json;
            }
            else
            {
                ViewData["result"] = "Error:" + response.StatusCode;
            }

            return View("Secure");
        }
    }
}