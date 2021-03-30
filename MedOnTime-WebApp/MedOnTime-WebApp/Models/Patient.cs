﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MedOnTime_WebApp.Models
{
    public class Patient
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int PatientID { get; set; }
        public int CaretakerID { get; set; } // for caretaker
        
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
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your age")]
        [BsonElement("Age")]
        public int Age { get; set; }
        public List<int> MedicationIDs { get; set; }
        public List <int> PrescriptionIDs { get; set; }

        public Patient() { }

        public Patient(int patientID, string firstName, string lastName, string email, string phoneNum, int age)
        {
            this.PatientID = patientID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.Age = age;
            this.MedicationIDs = new List<int>();
            this.CaretakerID = 0;
            this.PrescriptionIDs = new List<int>();
        }
    }
}
