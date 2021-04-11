using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

/*
 For Adding Patients from Caretaker Account, 2 Steps:
    Allow caretaker to add patient once logged in and display patients if there's any
    Only allow med to be added in a patient profile
 */

namespace MedOnTime_WebApp.Controllers
{
    public class PatientController : Controller
    {
        public PatientController()
        {
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            ViewBag.Message = "";
            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> PatientList()
        {
            List<Patient> existingPatients = new List<Patient>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI?caretakerID=" + LoginStatus.LogginedUser.CaretakerID))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);
                }
            }
            return View(existingPatients);
        }

        

        public async System.Threading.Tasks.Task<ActionResult> PatientLog(string Id)
        {
            List<Log> existingLogs = new List<Log>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/LogAPI?patientID=" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingLogs = JsonConvert.DeserializeObject<List<Log>>(apiRes);
                }
            }
            return View(existingLogs);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> AddPatient(Patient formResponse)
        {

            ViewBag.Message = "";
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine(formResponse.FirstName + ", " + formResponse.LastName + ", " + 
                                                    formResponse.Email + ", " + formResponse.PhoneNum + ", " + formResponse.Age);
                try
                {
                    //List<Patient> existingPatients = _patientCollection.AsQueryable<Patient>().ToList();

                    List<Patient> existingPatients = new List<Patient>();
                    // Get the existing Patients with GET method
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/PatientAPI"))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                            existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);
                        }
                    }

                    foreach (var patient in existingPatients)
                    {
                        Console.WriteLine(patient.LastName);
                        // if the response email is the same as an existing patient's email
                        if (formResponse.Email.Equals(patient.Email))
                        {
                            ViewBag.Message = formResponse.Email + " is associated to PID#" + patient.PatientID + ", " + patient.FirstName + ", " + patient.LastName + " already.";
                            return View(formResponse);
                        }
                    }

                    // if any existing patients, increment the new patient's patient ID
                    if (existingPatients.Count == 0)
                        formResponse.PatientID = 1;
                    else
                        formResponse.PatientID = existingPatients[existingPatients.Count - 1].PatientID + 1;
                    // Add new Patient with POST method
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync("https://medontime-api.herokuapp.com/api/PatientAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }
                    await LoginStatus.LoadPatients();
                    return RedirectToAction("Index", "Home");
                }
                catch {
                    Console.WriteLine("Is there any errors?");
                }
            }
            return View(formResponse);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> PatientDetails(string Id)
        {
            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            LoginStatus.SelectedPatient = patient;
            return View(patient);
        }

        // for going to EditPatient view to update info
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> EditPatient(string Id)
        {

            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(patient);
        }

        // post update method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> EditPatient(string Id, Patient formResponse)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", content))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("PatientList");
            }
            else
            {
                return View(formResponse);
            }
        } // end of EditPatient

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> DeletePatient(string Id)
        {
            Patient patient = new Patient();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<Patient>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> DeletePatient(string Id, Patient formResponse)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://medontime-api.herokuapp.com/API/PatientAPI/" + Id))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }
                return RedirectToAction("PatientList");
            }
            catch { return View(formResponse); }
        } // end of DeletePatient
    }
}
