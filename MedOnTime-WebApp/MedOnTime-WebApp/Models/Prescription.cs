using System;

namespace MedOnTime_WebApp.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public string PatientID { get; set; }
        public int HoursBetween { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }
        public int Picture { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Prescription(int prescriptionID, string patientID, int hoursBetween, string doctorName, string description, int picture, DateTime expiryDate)
        {
            this.PrescriptionID = prescriptionID;
            this.PatientID = patientID;
            this.HoursBetween = hoursBetween;
            this.DoctorName = doctorName;
            this.Description = description;
            this.Picture = picture;
            this.ExpiryDate = expiryDate;
        }
    }
}
