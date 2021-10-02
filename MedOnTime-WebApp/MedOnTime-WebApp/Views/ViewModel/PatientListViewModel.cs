using MedOnTime_WebApp.Models;
using System.Collections.Generic;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class PatientListViewModel
    {
        public List<Patient> Patients { get; set; }
        public int? CaretakerID { get; set; }
    }
}
