using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Caretaker
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNum { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
        public List<string> PatientIDs { get; set; }

        public List<Patient> Patients { get; set; }
        Caretaker() { }
        Caretaker(string firstName, string lastName, string email, int phoneNum, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.Age = age;
            this.PatientIDs = new List<string>();
            this.Patients = new List<Patient>();
        }
    }
}
