using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult QRCode()
        {
            return View();
        }
    }
}
