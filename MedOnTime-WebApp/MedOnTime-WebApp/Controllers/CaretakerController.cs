using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        // goes to AddPatient view
        public IActionResult AddPatient()
        {
            return View("./Patient/AddPatient");
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public IActionResult Register(Caretaker formResponse)
        {
            ViewBag.Message = "";
           
            if (ModelState.IsValid)
            {
                
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + formResponse.Email + "," + formResponse.PhoneNum);
                try
                {
                    List<Caretaker> existingCaretakers = _caretakerCollection.AsQueryable<Caretaker>().ToList();
                    foreach (var caretaker in existingCaretakers)
                    {
                        Console.WriteLine(caretaker.Username);
                        if (formResponse.Username.Equals(caretaker.Username))
                        {
                            ViewBag.Message = "Username " + formResponse.Username + " is not avaliable.";
                            return View(formResponse);
                        }
                    }
                    
                    if (existingCaretakers.Count == 0)
                        formResponse.CaretakerID = 1;
                    else
                        formResponse.CaretakerID = existingCaretakers[existingCaretakers.Count - 1].CaretakerID + 1;

                    // Hash the password
                    using (SHA256 sha256hash = SHA256.Create())
                        formResponse.PasswordHash = GetHash(sha256hash, formResponse.Password);

                    formResponse.Password = "";
                    _caretakerCollection.InsertOne(formResponse);
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
        public IActionResult Login(string email, string password)
        {
            if (email != null && password != null)
            {
                string pwdHash;
                using (SHA256 sha256hash = SHA256.Create())
                    pwdHash = GetHash(sha256hash, password);

                List<Caretaker> existingCaretakers = _caretakerCollection.AsQueryable<Caretaker>().ToList();
                foreach (var caretaker in existingCaretakers)
                {
                    if (caretaker.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && VerifyHash(pwdHash,caretaker.PasswordHash))
                    {
                        LoginStatus.IsLoggedIn = true;
                        if (caretaker.PatientIDs == null)
                            caretaker.PatientIDs = new List<int>();
                        LoginStatus.LogginedUser = caretaker;
                        return RedirectToAction("Index", "Home");
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


        // Comes from MS documentation
        // ref https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1
        public string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

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
