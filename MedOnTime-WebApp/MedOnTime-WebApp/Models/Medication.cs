using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedOnTime_WebApp.Models
{
    public class Medication
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public int? PrescriptionID { get; set; }

        [BsonElement("MedID")]
        public int MedID { get; set; }

        /*
                [BsonElement("ctID")]
                public int ctID { get; set; }

                [BsonElement("PatientID")]
                public int PatientID { get; set; }*/

        [Required(ErrorMessage = "Please enter medication name")]
        [BsonElement("MedicationName")]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "Please enter the method of taking")]
        [BsonElement("MethodOfTaking")]
        public string MethodOfTaking { get; set; }

        public int? MedicationImage { get; set; }

        [Required(ErrorMessage = "Please enter the dosage")]
        [BsonElement("Dosage")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Please select the type of taking")]
        [BsonElement("MedicationType")]
        public string MedicationType { get; set; }

        [Required(ErrorMessage = "Please select the quantity")]
        [BsonElement("Quantity")]
        public int? Quantity { get; set; }

        public Medication() { }

        public Medication(int? prescriptionID, string medicationName, string methodOfTaking, int? medImage, string dosage, string medicationType, int quanitity)
        {
            this.PrescriptionID = prescriptionID;
            this.MedicationName = medicationName;
            this.MethodOfTaking = methodOfTaking;
            this.MedicationImage = medImage;
            this.Dosage = dosage;
            this.MedicationType = medicationType;
            this.Quantity = quanitity;
        }
    }
}
