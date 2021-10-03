using MedOnTime_WebApp.Models;
using MedOnTime_WebApp.Views.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace MedOnTime_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index(string caretakerObjID)
        {
            if (caretakerObjID != null)
            {
                Caretaker userObj = await LoginStatus.LoadCaretaker(caretakerObjID);
                List<Patient> patients = await LoginStatus.LoadPatients(userObj.CaretakerID);
                return View(new HomeViewModel { UserObj = userObj, Patients = patients });
            }
            else
            {
                return View(new HomeViewModel { UserObj = null, Patients = null });
            }
        }

        public IActionResult SchedulePicker()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult QRCode()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
