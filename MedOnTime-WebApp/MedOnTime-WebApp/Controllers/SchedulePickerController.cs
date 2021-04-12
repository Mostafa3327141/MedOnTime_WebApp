using Microsoft.AspNetCore.Mvc;

namespace MedOnTime_WebApp.Controllers
{
    public class SchedulePickerController : Controller
    {
        public IActionResult SchedulePicker()
        {
            return View();
        }
    }
}
