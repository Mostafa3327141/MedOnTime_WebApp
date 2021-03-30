using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {
        private IMongoCollection<Medication> _medicationCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public MedicationController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _medicationCollection = database.GetCollection<Medication>("Medication");
        }

        [HttpGet]
        public IEnumerable<Medication> Get() => _medicationCollection.AsQueryable<Medication>().ToEnumerable();

        [HttpGet("{id}")]
        public ActionResult<Medication> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Value must be passed in the request body");
            }
            return Ok(_medicationCollection.AsQueryable<Medication>().SingleOrDefault(x => x.Id == id));
        }
    }
}
