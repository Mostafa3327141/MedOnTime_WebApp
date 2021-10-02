using MedOnTime_WebApp.Models;
using System.Collections.Generic;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class MedicationListViewModel
    {
        public List<Medication> Medications { get; set; }
        public Patient Patient { get; set; }
    }
}
