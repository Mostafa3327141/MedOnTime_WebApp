using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MedOnTime_WebApp.Models
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("PatientID")]
        public string PatientID { get; set; }

        [BsonElement("TimeTake")]
        public DateTime TimeTake { get; set; }

        [BsonElement("MedicationID")]
        public string MedicationID { get; set; }
    }
}
