using System.ComponentModel.DataAnnotations;

namespace MedOnTime_WebApp.Models
{
    public class Medication
    {
        public int? PrescriptionID { get; set; }
        public int _id { get; set; }
        [Required(ErrorMessage = "Please enter medication name")]
        public string MedicationName { get; set; }
        [Required(ErrorMessage = "Please enter the method of taking")]
        public string MethodOfTaking { get; set; }
        public int? MedicationImage { get; set; }
        [Required(ErrorMessage = "Please enter the dosage")]
        public string Dosage { get; set; }
        [Required(ErrorMessage = "Please select the type of taking")]
        public string MedicationType { get; set; }
        [Required(ErrorMessage = "Please select the quantity")]
        public int? Quantity { get; set; }

        public Medication() { }

        public Medication(int? prescriptionID, int _id, string medicationName, string methodOfTaking, int? medImage, string dosage, string medicationType, int quanitity)
        {
            this.PrescriptionID = prescriptionID;
            this._id = _id;
            this.MedicationName = medicationName;
            this.MethodOfTaking = methodOfTaking;
            this.MedicationImage = medImage;
            this.Dosage = dosage;
            this.MedicationType = medicationType;
            this.Quantity = quanitity;
        }
    }
}
