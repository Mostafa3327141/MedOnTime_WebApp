using System;

namespace MedOnTime_WebApp.Models
{
    public class Alert
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string PatientID { get; set; }

       public Alert(string message, DateTime date, DateTime time, string patientID)
        {
            this.Message = message;
            this.Date = date;
            this.Time = time;
            this.PatientID = patientID;
        }
    }
}
