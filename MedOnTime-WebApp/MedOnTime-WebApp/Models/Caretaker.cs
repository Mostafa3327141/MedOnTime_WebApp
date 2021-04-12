using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedOnTime_WebApp.Models
{
    public class Caretaker
    {
        public string Id { get; set; }
        public int CaretakerID { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "please enter your phone number")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public List<int> PatientIDs { get; set; }

        public Caretaker() { }

        public Caretaker(string firstName, string lastName, string email, string phoneNum)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.PatientIDs = new List<int>();
        }
    }
}
