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

        public string MedicationImage { get; set; }

        public string ImageId { get; set; }

        [Required(ErrorMessage = "Please enter the unit")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Please select the quantity")]
        public int? Quantity { get; set; }

        public string Condition { get; set; }

        [Required(ErrorMessage = "Please enter time of taking the medication")]
        public string FirstDoseTime { get; set; }

        [Required(ErrorMessage = "Please enter time of taking the medication")]
        public int? HoursBetween { get; set; }

        [Required(ErrorMessage = "Please enter the frequency")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Please select a shape for this medication")]
        public string Shape { get; set; }

        public List<DateTime> Times { get; set; }

        public Medication() { }
        public Medication(string medicationName, string medImage, string unit, int quantity, string firstDoseTime, string frequency, string shape,int hoursBetween)
        {
            this.MedicationName = medicationName;
            this.Unit = unit;
            this.Quantity = quantity;
            this.FirstDoseTime = firstDoseTime;
            this.Frequency = frequency;
            this.Shape = shape;
            this.HoursBetween = hoursBetween;
            this.Times = new List<DateTime>();
        }
    }
}
