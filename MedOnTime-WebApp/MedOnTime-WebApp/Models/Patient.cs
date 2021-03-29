using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MedOnTime_WebApp.Models
{
    public class Patient
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [BsonElement("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [BsonElement("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number")]
        [BsonElement("PhoneNum")]
        public int PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your age")]
        [BsonElement("Age")]
        public int Age { get; set; }
        public List<Medication> Medicines { get; set; }
        public List<Caretaker> Caretakers { get; set; }
        public List <Prescription> Prescriptions { get; set; }

        public Patient() { }

        public Patient(int patientID, string firstName, string lastName, string email, int phoneNum, int age)
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
