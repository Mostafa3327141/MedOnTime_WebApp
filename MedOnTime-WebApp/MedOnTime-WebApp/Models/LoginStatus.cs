using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace MedOnTime_WebApp.Models
{
    public static class LoginStatus
    {
        public static string ApiKey { get { return "key=sH5O!2cdOqP1^"; } }

        public static async System.Threading.Tasks.Task<List<Patient>> LoadPatients(int caretakerID)
        {
            List<Patient> Patients = new List<Patient>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/PatientAPI?" + ApiKey))
                {
                    // Load the patients that's under this caretaker
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    List<Patient> existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);

                    foreach (var patient in existingPatients)
                        if (patient.CaretakerID == caretakerID)
                        {
                            Patients.Add(patient);
                        }
                }
            }

            return Patients;
        }

        public static async System.Threading.Tasks.Task<Patient> LoadPatient(int patientID)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/PatientAPI?" + ApiKey))
                {
                    // Load the patients that's under this caretaker
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    List<Patient> existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);

                    foreach (var patient in existingPatients)
                        if (patient.PatientID == patientID)
                        {
                            return patient;
                        }
                }
            }

            return null;
        }

        public static async System.Threading.Tasks.Task<Caretaker> LoadCaretaker(string caretakerObjID)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/CaretakerAPI/" + caretakerObjID + "?" + ApiKey))
                {
                    // Load the patients that's under this caretaker
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    Caretaker caretaker = JsonConvert.DeserializeObject<Caretaker>(apiRes);

                    return caretaker;
                }
            }

            return null;
        }
    }
}
