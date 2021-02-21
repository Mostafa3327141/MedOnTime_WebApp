using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Credintial
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Credintial(string email, string username, string password)
        {
            this.Email = email;
            this.Username = username;
            this.Password = password;
        }
    }
}
