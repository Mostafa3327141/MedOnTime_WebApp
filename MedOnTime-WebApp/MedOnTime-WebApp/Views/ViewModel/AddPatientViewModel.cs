using MedOnTime_WebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MedOnTime_WebApp.Views.ViewModel
{
    public class AddPatientViewModel
    {
        public Patient Patient { get; set; }
        public int? CaretakerID { get; set; }
        public string CaretakerObjID { get; set; }

        [Required(ErrorMessage = "Please provide a temporary password.")]
        public string TempPassword { get; set; }
    }
}
