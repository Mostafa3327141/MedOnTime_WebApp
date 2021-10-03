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
                        formResponse.PasswordHash = GetHash(sha256hash, formResponse.Password, formResponse.Email.ToLower());

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
                string pwdHash;
                using (SHA256 sha256hash = SHA256.Create())
                    pwdHash = GetHash(sha256hash, password, email.ToLower());

                List<Caretaker> existingCaretakers = new List<Caretaker>();

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
                    if (caretaker.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && VerifyHash(pwdHash,caretaker.PasswordHash))
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


        // Comes from MS documentation
        // ref https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1
        public string GetHash(HashAlgorithm hashAlgorithm, string password, string email)
        {

            // Convert the password string with email as salt to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes((password + email)));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public bool VerifyHash(string input, string hash)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(input, hash) == 0;
        }
    }
}
