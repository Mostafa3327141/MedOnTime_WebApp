using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace MedOnTime_WebApp.Models
{
    public static class LoginStatus
    {
        public static string ApiKey { get { return "key=sH5O!2cdOqP1^"; } }
        public static bool IsLoggedIn { get; set; } = false;
        public static Caretaker LogginedUser { get; set; }
        public static List<Patient> Patients { get; set; }
        public static Patient SelectedPatient { get; set; }

        public static async System.Threading.Tasks.Task LoadPatients()
        {
            Patients = new List<Patient>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://medontime-api.herokuapp.com/api/PatientAPI?" + ApiKey))
                {
                    // Load the patients that's under this caretaker
                    string apiRes = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(apiRes);
                    List<Patient> existingPatients = JsonConvert.DeserializeObject<List<Patient>>(apiRes);

                    foreach (var patient in existingPatients)
                        if (patient.CaretakerID == LogginedUser.CaretakerID)
                        {
                            Patients.Add(patient);
                        }
                }
            }
        }

        public static void Logout()
        {
            IsLoggedIn = false;
            LogginedUser = null;
            Patients = null;
            SelectedPatient = null;
        }
    }
}
