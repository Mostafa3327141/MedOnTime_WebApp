using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MedOnTime_WebApp.Controllers
{
    public class CaretakerController : Controller
    {
        private IMongoCollection<Caretaker> _caretakerCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public CaretakerController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _caretakerCollection = database.GetCollection<Caretaker>("Caretaker");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Caretaker formResponse)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + formResponse.Email + "," + formResponse.PhoneNum);
                try
                {
                    List<Caretaker> existingCaretakers = _caretakerCollection.AsQueryable<Caretaker>().ToList();
                    if (existingCaretakers.Count == 0)
                        formResponse.ctID = 1;
                    else
                        formResponse.ctID = existingCaretakers[existingCaretakers.Count - 1].ctID + 1;

                    _caretakerCollection.InsertOne(formResponse);
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
