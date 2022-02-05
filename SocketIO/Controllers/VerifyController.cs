using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocketIO.Controllers
{
    public class VerifyController : Controller
    {
        public IActionResult Index()
        {
            bool isLogin = false;
            string guestName = "";
            if (User.Identity.IsAuthenticated)
            {
                guestName = (from c in User.Claims
                             where c.Type == ClaimTypes.Name
                             select c).FirstOrDefault().Value;
                isLogin = true;
            }
            else
            {
                
            }
            var jsonResult = new { isLogin, guestName };
            return Json(jsonResult);
        }
    }
}
