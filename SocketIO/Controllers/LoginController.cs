using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SocketIO.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            bool isLogin = false;
            StringValues guestName = "";
            StringValues password = "";
            Request.Form.TryGetValue("guestName", out guestName);
            Request.Form.TryGetValue("password", out password);
            if (password == "")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, guestName.ToString() ),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddYears(100),
                    IsPersistent = false,
                };
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties).Wait();
                isLogin = true;
            }
            else
            { 

            }
            var jsonResult = new { isLogin, guestName = guestName.ToString() };
            
            return Json(jsonResult);
        }
    }
}
