using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ClientSideApp.Models;
using ClientSideApp.ViewModels;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace ClientSideApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getTokenUrl = ConfigurationManager.AppSettings["ApiRegisterUrl"];

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Name", model.Name),
                        new KeyValuePair<string, string>("Email", model.UserName),
                        new KeyValuePair<string, string>("Password", model.Password),
                        new KeyValuePair<string, string>("ConfirmPassword", model.ConfirmPassword)
                    });

                    HttpResponseMessage result = httpClient.PostAsync(getTokenUrl, content).Result;

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var getTokenUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.UserName),
                    new KeyValuePair<string, string>("password", model.Password)
                });

                HttpResponseMessage result = httpClient.PostAsync(getTokenUrl, content).Result;

                string resultContent = result.Content.ReadAsStringAsync().Result;

                var token = JsonConvert.DeserializeObject<Access>(resultContent);

                AuthenticationProperties options = new AuthenticationProperties();

               //options.AllowRefresh = true;
                options.IsPersistent = true;
                options.ExpiresUtc = DateTime.UtcNow.AddSeconds(int.Parse(token.expires_in));

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.Name),
                    new Claim(ClaimTypes.Email, model.UserName),
                    new Claim("AcessToken", string.Format("Bearer {0}", token.access_token)),
                };

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                Request.GetOwinContext().Authentication.SignIn(options, identity);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");

            return RedirectToAction("Login");
        }
    }
}
