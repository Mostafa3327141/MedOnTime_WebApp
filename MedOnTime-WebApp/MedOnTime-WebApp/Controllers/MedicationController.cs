using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace MedOnTime_WebApp.Controllers
{
    public class MedicationController : Controller
    {
        public MedicationController()
        {
        }

        public async System.Threading.Tasks.Task<ActionResult> MedicationList()
        {
            List<Medication> existingMeds = new List<Medication>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/API/MedicationAPI"))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingMeds = JsonConvert.DeserializeObject<List<Medication>>(apiRes);
                }
            }
            return View(existingMeds);
        }

        [HttpGet]
        public IActionResult MedicationForm()
        {
            return View();
        }

        // Method for API Testing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationForm(Medication formResponse)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                Console.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);
                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync("https://localhost:44338/API/MedicationAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
                catch { return View(formResponse); }
            }
            return View(formResponse);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(string Id)
        {

            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/API/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(string objID, Medication formResponse)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://localhost:44338/API/MedicationAPI", content))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("MedicationList");
            } 
            else
            {
                return View(formResponse);
            }
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDetails(string Id)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/API/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string Id)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44338/API/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(med);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string objID, Medication formResponse)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://localhost:44338/API/MedicationAPI/" + objID))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("MedicationList");
            }
            catch { return View(formResponse); }
        }
    }
}
