using MedOnTime_WebApp.Models;
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
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI?" + LoginStatus.ApiKey))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    existingMeds = JsonConvert.DeserializeObject<List<Medication>>(apiRes);
                }
            }

            foreach (var med in existingMeds)
            {
                if (med.PatientID == LoginStatus.SelectedPatient.PatientID)
                    patientMeds.Add(med);
            }

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

                    // Remove selected shape into the list of unselected shape
                    foreach (Shape s in LoginStatus.SelectedPatient.UnSelectedShapes)
                    {
                        if (s.ShapeName == formResponse.Shape)
                        {
                            LoginStatus.SelectedPatient.UnSelectedShapes.Remove(s);
                            break;
                        }
                    }

                    // Update patient's unselected shapes
                    StringContent patientContent = new StringContent(JsonConvert.SerializeObject(LoginStatus.SelectedPatient), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
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
        public async System.Threading.Tasks.Task<IActionResult> MedicationEdit(Medication formResponse, string oldShape, IFormFile medImage)
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

                if (!formResponse.Shape.Equals(oldShape))
                {
                    LoginStatus.SelectedPatient.UnSelectedShapes.Add(getShapeByName(oldShape));

                    // Remove selected shape into the list of unselected shape
                    foreach (Shape s in LoginStatus.SelectedPatient.UnSelectedShapes)
                    {
                        if (s.ShapeName == formResponse.Shape)
                        {
                            LoginStatus.SelectedPatient.UnSelectedShapes.Remove(s);
                            break;
                        }
                    }
                } 

                // Update patient's unselected shapes
                StringContent patientContent = new StringContent(JsonConvert.SerializeObject(LoginStatus.SelectedPatient), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
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
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/API/MedicationAPI/" + Id + "?" + LoginStatus.ApiKey))
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
        public async System.Threading.Tasks.Task<IActionResult> MedicationDelete(string objID, string medShape, Medication formResponse)
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

                LoginStatus.SelectedPatient.UnSelectedShapes.Add(getShapeByName(medShape));

                // Update patient's unselected shapes
                StringContent patientContent = new StringContent(JsonConvert.SerializeObject(LoginStatus.SelectedPatient), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync("https://medontime-api.herokuapp.com/API/PatientAPI", patientContent))
                    {
                        string apiRes = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine(apiRes);
                    }
                }

                return RedirectToAction("MedicationList");
            }
            catch { return View(formResponse); }
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
