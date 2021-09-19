using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedOnTime_WebApp.Models
{
    public class Medication
    {
        public string Id { get; set; }

        public string CaretakerID { get; set; }

        public int PatientID { get; set; }

        [Required(ErrorMessage = "Please enter medication name")]
        public string MedicationName { get; set; }

        [Required(ErrorMessage = "Please enter the method of taking")]
        public string MethodOfTaking { get; set; }

        public string MedicationImage { get; set; }

        public string ImageId { get; set; }

        [Required(ErrorMessage = "Please enter the dosage")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Please select the type of taking")]
        public string MedicationType { get; set; }

        [Required(ErrorMessage = "Please select the quantity")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Please enter time of taking the medication")]
        public string FirstDoseTime { get; set; }

        //[Required(ErrorMessage = "Please enter time of taking the medication")]
        public int HoursBetween { get; set; }

        [Required(ErrorMessage = "Please enter the frequency")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Please select a shape for this medication")]
        public string Shape { get; set; }

        public List<DateTime> Times { get; set; }

        public Medication() { }
        public Medication(string medicationName, string methodOfTaking, string medImage, string dosage, string medicationType, int quantity, string firstDoseTime, string frequency, string shape,int hoursBetween)
        {
            this.MedicationName = medicationName;
            this.MethodOfTaking = methodOfTaking;
            this.MedicationImage = medImage;
            this.Dosage = dosage;
            this.MedicationType = medicationType;
            this.Quantity = quantity;
            this.FirstDoseTime = firstDoseTime;
            this.Frequency = frequency;
            this.Shape = shape;
            this.HoursBetween = hoursBetween;
            this.Times = new List<DateTime>();
        }
    }
}
