using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Sample
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("SampleName")]
        public string SampleName { get; set; }

        [BsonElement("SampleDescription")]
        public string SampleDescription { get; set; }

        [BsonElement("SampleQuantity")]
        public int SampleQuantity { get; set; }
    }
}
