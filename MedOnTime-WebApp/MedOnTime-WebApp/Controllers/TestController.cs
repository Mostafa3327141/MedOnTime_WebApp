using MedOnTime_WebApp.Models;
using MedOnTime_WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Controllers
{

    public class TestController : Controller
    {
        private readonly TestService _subSvc;

        public TestController(TestService TestService)
        {
            _subSvc = TestService;
        }

        [AllowAnonymous]
        public ActionResult<IList<Test>> Index() => View(_subSvc.Read());

        [HttpGet]
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult<Test> Create(Test Test)
        {
            Test.Created = Test.LastUpdated = DateTime.Now;
            //Test.UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Test.UserId = "1";
            Test.UserName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                _subSvc.Create(Test);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult<Test> Edit(string id) =>
            View(_subSvc.Find(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Test Test)
        {
            Test.LastUpdated = DateTime.Now;
            Test.Created = Test.Created.ToLocalTime();
            if (ModelState.IsValid)
            {
                if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value != Test.UserId)
                {
                    return Unauthorized();
                }
                _subSvc.Update(Test);
                return RedirectToAction("Index");
            }
            return View(Test);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _subSvc.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
