using System;

namespace MedOnTime_WebApp.Models
{
    public class Log
    {
        public string Id { get; set; }

        public string PatientID { get; set; }

        /*        [BsonElement("TimeTake")]
                public DateTime TimeTake { get; set; }*/

        public string MedicationID { get; set; }

        public string MedicationName { get; set; }
    }
}
