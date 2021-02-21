using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Log
    {
        public string PatientID { get; set; }
        public DateTime TimeTake { get; set; }
        public int MedicationID { get; set; }

        Log(string patientID, DateTime timeTake, int medicationID)
        {
            this.PatientID = patientID;
            this.TimeTake = timeTake; 
            this.MedicationID = medicationID;
        }
    }
}
