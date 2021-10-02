using MedOnTime_WebApp.Models;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class AddPatientViewModel
    {
        public Patient Patient { get; set; }
        public int? CaretakerID { get; set; }
        public string CaretakerObjID { get; set; }
    }
}
