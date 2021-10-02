using MedOnTime_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class HomeViewModel
    {
        public Caretaker UserObj { get; set; }
        public List<Patient> Patients { get; set; }
    }
}
