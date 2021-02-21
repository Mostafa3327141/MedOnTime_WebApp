using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    public class Subscription
    {
        public string ID { get; set; }
        public DateTime CaretakerID { get; set; }

        public Subscription(string id, DateTime caretakerID)
        {
            this.ID = id;
            this.CaretakerID = caretakerID;
        }
    }
}
