using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            /*
             * Checkes if credential entered matches the records
             * if matches then change the app state to logined
             * else rejects and pops a credential invalid message
             */
            return View();
        }
    }
}
