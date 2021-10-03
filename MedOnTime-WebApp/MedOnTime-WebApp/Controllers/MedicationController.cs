using MedOnTime_WebApp.Models;
using MedOnTime_WebApp.Views.ViewModel;
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
        public async System.Threading.Tasks.Task<ActionResult> MedicationList(int patientID)
        {
            List<Medication> existingMeds = new List<Medication>();
            List<Medication> patientMeds = new List<Medication>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingMeds = JsonConvert.DeserializeObject<List<Medication>>(apiRes);
                }
            }

            foreach (var med in existingMeds)
            {
                if (med.PatientID == patientID)
                    patientMeds.Add(med);
            }

            Patient patient = await LoginStatus.LoadPatient(patientID);

            return View(new MedicationListViewModel{ Medications = patientMeds, Patient = patient });
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationForm(int patientID, string caretakerObjID)
        {
            Patient patient = await LoginStatus.LoadPatient(patientID);
            Caretaker caretaker = await LoginStatus.LoadCaretaker(caretakerObjID);
            return View(new MedicationFormViewModel { Patient = patient, Caretaker = caretaker});
        }

        // Method for API Testing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationForm(MedicationFormViewModel formResponse, IFormFile medImage)
        {
            if (ModelState.IsValid)
            {

                if (medImage != null)
                {
                    // prepping medImage for serialization
                    var bytes = await medImage.GetBytes();
                    formResponse.Medication.MedicationImage = Convert.ToBase64String(bytes);
                } else
                {
                    formResponse.Medication.MedicationImage = null;
                }

               /* if (formResponse.Frequency == "Every Day")
                    formResponse.HoursBetween = 24; // only if selecting Every Day option*/

                // create a new list for the newly binded medication object
                formResponse.Medication.Times = new List<DateTime>();
                formResponse.Medication.Times.Add(DateTime.Parse(formResponse.Medication.FirstDoseTime.Insert(5, ":00")));


                formResponse.Caretaker = await LoginStatus.LoadCaretaker(formResponse.Medication.CaretakerID);
                formResponse.Patient = await LoginStatus.LoadPatient(formResponse.Medication.PatientID);

                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse.Medication), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync("https://medontime-api.herokuapp.com/API/MedicationAPI", content))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }

                    // Remove selected shape into the list of unselected shape
                    foreach (Shape s in formResponse.Patient.UnSelectedShapes)
                    {
                        if (s.ShapeName == formResponse.Medication.Shape)
                        {
                            formResponse.Patient.UnSelectedShapes.Remove(s);
                            break;
                        }
                    }

                    // Update patient's unselected shapes
                    StringContent patientContent = new StringContent(JsonConvert.SerializeObject(formResponse.Patient), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
                        {
                            string apiRes = await response.Content.ReadAsStringAsync();
                            System.Diagnostics.Debug.WriteLine(apiRes);
                        }
                    }

                    return RedirectToAction("MedicationList", new { patientID = formResponse.Patient.PatientID });
                }
                catch 
                {
                    return View(formResponse); 
                }
            }
            return View(formResponse);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(string Id)
        {

            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id + "?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            Caretaker caretaker = await LoginStatus.LoadCaretaker(med.CaretakerID);
            Patient patient = await LoginStatus.LoadPatient(med.PatientID);
            return View(new MedicationEditViewModel { Medication = med , Caretaker = caretaker, Patient = patient });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(MedicationEditViewModel formResponse, string oldShape, IFormFile medImage)
        {
            if (ModelState.IsValid)
            {
                if (medImage != null)
                {
                    // prepping medImage for serialization
                    var bytes = await medImage.GetBytes();
                    formResponse.Medication.MedicationImage = Convert.ToBase64String(bytes);
                }

                formResponse.Caretaker = await LoginStatus.LoadCaretaker(formResponse.Medication.CaretakerID);
                formResponse.Patient = await LoginStatus.LoadPatient(formResponse.Medication.PatientID);

                StringContent content = new StringContent(JsonConvert.SerializeObject(formResponse.Medication), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/MedicationAPI", content))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }

                if (!formResponse.Medication.Shape.Equals(oldShape))
                {
                    formResponse.Patient.UnSelectedShapes.Add(getShapeByName(oldShape));

                    // Remove selected shape into the list of unselected shape
                    foreach (Shape s in formResponse.Patient.UnSelectedShapes)
                    {
                        if (s.ShapeName == formResponse.Medication.Shape)
                        {
                            formResponse.Patient.UnSelectedShapes.Remove(s);
                            break;
                        }
                    }
                } 

                // Update patient's unselected shapes
                StringContent patientContent = new StringContent(JsonConvert.SerializeObject(formResponse.Patient), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }

                return RedirectToAction("MedicationList", new { patientID = formResponse.Patient.PatientID });
            } 
            else
            {
                return View(formResponse);
            }
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDetails(string Id, int patientID)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id + "?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    med = JsonConvert.DeserializeObject<Medication>(apiRes);
                    if (med.MedicationImage != null)
                    {
                        var binary = Convert.FromBase64String(med.MedicationImage);
                        System.IO.File.WriteAllBytes("./wwwroot/img/" + med.Id + ".jpg", binary); // TODO: Detect image file type before creating file.
                    }
                    System.Diagnostics.Debug.WriteLine(apiRes);
                }
            }
            return View(new MedicationDetailsViewModel { Medication = med, PatientID = patientID });
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string Id)
        {
            Medication med = new Medication();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id + "?" + LoginStatus.ApiKey))
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
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string objID, string medShape, int patientID)
        {            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + objID + "?" + LoginStatus.ApiKey))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }

                Patient patient = await LoginStatus.LoadPatient(patientID);
                patient.UnSelectedShapes.Add(getShapeByName(medShape));

                // Update patient's unselected shapes
                StringContent patientContent = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }

                return RedirectToAction("MedicationList", new { patientID = patientID });
            }
            catch { return View(objID); }
        }

        private Shape getShapeByName(string name)
        {
            string medShapeDisplay = "";
            switch (name)
            {
                case "circle":
                    medShapeDisplay = "Circle";
                    break;
                case "oval":
                    medShapeDisplay = "Oval";
                    break;
                case "triangle":
                    medShapeDisplay = "Triangle";
                    break;
                case "heart":
                    medShapeDisplay = "Heart";
                    break;
                case "pentagon":
                    medShapeDisplay = "Pentagon";
                    break;
                case "hexagon":
                    medShapeDisplay = "Hexagon";
                    break;
                case "octagon":
                    medShapeDisplay = "Octagon";
                    break;
                case "rightTri":
                    medShapeDisplay = "Right Triangle";
                    break;
                case "sTri":
                    medShapeDisplay = "Scalene Triangle";
                    break;
                case "square":
                    medShapeDisplay = "Square";
                    break;
                case "rectangle":
                    medShapeDisplay = "Rectangle";
                    break;
                case "parallelogram":
                    medShapeDisplay = "Parallelogram";
                    break;
                case "trapezuim":
                    medShapeDisplay = "Trapezuim";
                    break;
                case "rhombus":
                    medShapeDisplay = "Rhombus";
                    break;
                case "4star":
                    medShapeDisplay = "4 Pointed Star";
                    break;
                case "star":
                    medShapeDisplay = "5 Pointed Star";
                    break;
                case "6star":
                    medShapeDisplay = "6 Pointed Star";
                    break;
            }

            return new Shape { ShapeName = name, ShapeDisplay = medShapeDisplay };
        }
    }
}
