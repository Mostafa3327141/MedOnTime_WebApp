using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNum { get; set; }
        public int Age { get; set; }
        public List<Medication> Medicines { get; set; }
        public List<Caretaker> Caretakers { get; set; }
        public List <Prescription> Prescriptions { get; set; }

        Patient() { }

        Patient(int patientID, string firstName, string lastName, string email, int phoneNum, int age)
        {
            this.PatientID = patientID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.Age = age;
            this.Medicines = new List<Medication>();
            this.Caretakers = new List<Caretaker>();
            this.Prescriptions = new List<Prescription>();
        }
    }
}
