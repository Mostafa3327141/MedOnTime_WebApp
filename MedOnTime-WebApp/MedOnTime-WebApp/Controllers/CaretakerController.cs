using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace MedOnTime_WebApp.Controllers
{
    public class CaretakerController : Controller
    {
        public CaretakerController()
        {
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Message = "";
            return View();
        }

        public IActionResult RecoverPassword(string email)
        {
            //TODO: Send an email to recover the user's password.
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Register(Caretaker formResponse)
        {
            ViewBag.Message = "";
           
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + formResponse.Email + "," + formResponse.PhoneNum);
                try
                {
                    List<Caretaker> existingCaretakers = new List<Caretaker>();
                    // Get the existing Caretakers with GET method
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/CaretakerAPI?" + LoginStatus.ApiKey))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                            existingCaretakers = JsonConvert.DeserializeObject<List<Caretaker>>(apiRes);
                        }
                    }
                    // Check if there's any entries that have the same email
                    foreach (var caretaker in existingCaretakers)
                    {
                        if (formResponse.Email.Equals(caretaker.Email, StringComparison.OrdinalIgnoreCase))
                        {
                            ViewBag.Message = "Username " + formResponse.Email + " is not avaliable.";
                            return View(formResponse);
                        }
                    }
                    
                    if (existingCaretakers.Count == 0)
                        formResponse.CaretakerID = 1;
                    else
                        formResponse.CaretakerID = existingCaretakers[existingCaretakers.Count - 1].CaretakerID + 1;

                    // Hash the password
                    using (SHA256 sha256hash = SHA256.Create())
                        formResponse.PasswordHash = LoginStatus.GetHash(sha256hash, formResponse.Password, formResponse.Email.ToLower());

                    formResponse.Password = "";

                    // Add new Caretaker with POST method
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync("https://medontime-api.herokuapp.com/api/CaretakerAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch { Console.WriteLine("See an error?"); }
            }
            return View(formResponse);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Login(string email, string password)
        {
            if (email != null && password != null)
            {
                // hash the password
                string pwdHash;
                using (SHA256 sha256hash = SHA256.Create())
                    pwdHash = LoginStatus.GetHash(sha256hash, password, email.ToLower());

                List<Caretaker> existingCaretakers = new List<Caretaker>();

                // load all existing caretakers
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/CaretakerAPI?" + LoginStatus.ApiKey))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                        existingCaretakers = JsonConvert.DeserializeObject<List<Caretaker>>(apiRes);
                    }
                }

                foreach (var caretaker in existingCaretakers)
                {
                    // Check if there is any entries' data matches with the provided data
                    if (caretaker.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && LoginStatus.VerifyHash(pwdHash,caretaker.PasswordHash))
                    {
                        return RedirectToAction("Index", "Home", new { caretakerObjID = caretaker.Id});
                    }
                }

                ViewBag.Message = "Either Email/password is incorrect.";
            }
            else
            {
                ViewBag.Message = "Please enter your credentials.";
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
