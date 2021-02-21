using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Medication
    {
        public int PrescriptionID { get; set; }
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string MethodOfTaking { get; set; }
        public int MedicationImage { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }

        public Medication(int prescriptionID, int medicationID, string medicationName, string methodOfTaking, int medImage, string dosage, int quanitity)
        {
            this.PrescriptionID = prescriptionID;
            this.MedicationID = medicationID;
            this.MedicationName = medicationName;
            this.MethodOfTaking = methodOfTaking;
            this.MedicationImage = medImage;
            this.Dosage = dosage;
            this.Quantity = quanitity;
        }
    }
}
