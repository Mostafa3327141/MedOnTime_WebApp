using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

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
        }

        // Comes from MS documentation
        // ref https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1
        public static string GetHash(HashAlgorithm hashAlgorithm, string password, string email)
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
        public static bool VerifyHash(string input, string hash)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(input, hash) == 0;
        }
    }
}
