using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedOnTime_WebApp.Models
{
    public class Caretaker
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ctID { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        [BsonElement("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [BsonElement("LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "please enter your phone number")]
        [BsonElement("PhoneNum")]
        public int? PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [BsonElement("Email")]
        public string Email { get; set; }

        /*[Required(ErrorMessage = "Please enter your age")]
        [BsonElement("Age")]
        public int? Age { get; set; }*/

        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }
        public List<string> PatientIDs { get; set; }

        public List<Patient> Patients { get; set; }
        public Caretaker() { }
        public Caretaker(string firstName, string lastName, string email, int phoneNum)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.PatientIDs = new List<string>();
            this.Patients = new List<Patient>();
        }
    }
}
