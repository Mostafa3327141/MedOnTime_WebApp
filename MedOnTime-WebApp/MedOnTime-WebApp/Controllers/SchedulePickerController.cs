﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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