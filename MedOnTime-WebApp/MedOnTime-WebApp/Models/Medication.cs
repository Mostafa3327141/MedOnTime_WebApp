using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MedOnTime_WebApp.Models
{
    public class Medication
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("CaretakerID")]
        public string CaretakerID { get; set; }
        [BsonElement("PatientID")]
        public int PatientID { get; set; }
        [Required(ErrorMessage = "Please enter medication name")]
        [BsonElement("MedicationName")]
        public string MedicationName { get; set; }
        [Required(ErrorMessage = "Please enter the method of taking")]
        [BsonElement("MethodOfTaking")]
        public string MethodOfTaking { get; set; }
        [BsonElement("MedicationImage")]
        public string MedicationImage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ImageId { get; set; }
        [Required(ErrorMessage = "Please enter the dosage")]
        [BsonElement("Dosage")]
        public string Dosage { get; set; }
        [Required(ErrorMessage = "Please select the type of taking")]
        [BsonElement("MedicationType")]
        public string MedicationType { get; set; }
        [Required(ErrorMessage = "Please select the quantity")]
        [BsonElement("Quantity")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Please enter time of taking the medication")]
        [BsonElement("FirstDoseTime")]
        public string FirstDoseTime { get; set; }

        //[Required(ErrorMessage = "Please enter time of taking the medication")]
        [BsonElement("HoursBetween")]
        public int HoursBetween { get; set; }

        [Required(ErrorMessage = "Please enter the Frequency")]
        [BsonElement("Frequency")]
        public string Frequency { get; set; }

        [BsonElement("Times")]
        public List<DateTime> Times { get; set; }

        public Medication() { }
        public Medication(string medicationName, string methodOfTaking, string medImage, string dosage, string medicationType, int quantity, string firstDoseTime, string frequency, int hoursBetween)
        {
            this.MedicationName = medicationName;
            this.MethodOfTaking = methodOfTaking;
            this.MedicationImage = medImage;
            this.Dosage = dosage;
            this.MedicationType = medicationType;
            this.Quantity = quantity;
            this.FirstDoseTime = firstDoseTime;
            this.Frequency = frequency;
            this.HoursBetween = hoursBetween;
            this.Times = new List<DateTime>();
        }
    }
}
