using MedOnTime_WebApp.Models;
using System.Collections.Generic;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class HomeViewModel
    {
        public Caretaker UserObj { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
