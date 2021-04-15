﻿using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MedOnTime_WebApp.Controllers
{
    public class MedicationController : Controller
    {
        public MedicationController()
        {
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> MedicationList()
        {
            List<Medication> existingMeds = new List<Medication>();
            List<Medication> patientMeds = new List<Medication>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI"))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingMeds = JsonConvert.DeserializeObject<List<Medication>>(apiRes);
                }
            }

            foreach (var med in existingMeds)
                if (med.PatientID == LoginStatus.SelectedPatient.PatientID)
                    patientMeds.Add(med);

            return View(patientMeds);
        }

        [HttpGet]
        public IActionResult MedicationForm()
        {
            return View();
        }

        // Method for API Testing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationForm(Medication formResponse, IFormFile medImage)
        {
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.MedicationName + ", " + formResponse.MethodOfTaking + ", " + formResponse.Dosage + ", " + formResponse.Quantity + ", " + formResponse.MedicationType);

                if (medImage != null)
                {
                    // prepping medImage for serialization
                    var bytes = await medImage.GetBytes();
                    formResponse.MedicationImage = Convert.ToBase64String(bytes);
                } else
                {
                    formResponse.MedicationImage = null;
                }

               /* if (formResponse.Frequency == "Every Day")
                    formResponse.HoursBetween = 24; // only if selecting Every Day option*/

                // create a new list for the newly binded medication object
                formResponse.Times = new List<DateTime>();
                formResponse.Times.Add(DateTime.Parse(formResponse.FirstDoseTime.Insert(5, ":00")));

                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync("https://medontime-api.herokuapp.com/API/MedicationAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }
                    return RedirectToAction("MedicationList");
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
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id))
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
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(Medication formResponse, IFormFile medImage)
        {
            if (ModelState.IsValid)
            {
                if (medImage != null)
                {
                    // prepping medImage for serialization
                    var bytes = await medImage.GetBytes();
                    formResponse.MedicationImage = Convert.ToBase64String(bytes);
                }

                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/MedicationAPI", content))
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
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    if (med.MedicationImage != null)
                    {
                        var binary = Convert.FromBase64String(med.MedicationImage);
                        System.IO.File.WriteAllBytes("./wwwroot/img/" + med.Id + ".jpg", binary); // TODO: Detect image file type before creating file.
                        //System.IO.File.WriteAllBytes("./wwwroot/img/test.jpg", binary); // TODO: Detect image file type before creating file.
                    }
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
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id))
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
                    using (var response = await httpClient.DeleteAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + objID))
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
