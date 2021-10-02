using MedOnTime_WebApp.Models;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class MedicationEditViewModel
    {
        public Patient Patient { get; set; }
        public Medication Medication { get; set; }
        public Caretaker Caretaker { get; set; }
    }
}
