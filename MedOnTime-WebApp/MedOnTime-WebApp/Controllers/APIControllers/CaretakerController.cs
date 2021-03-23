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
    public class CaretakerController : ControllerBase
    {
        private IMongoCollection<Caretaker> _caretakerCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public CaretakerController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _caretakerCollection = database.GetCollection<Caretaker>("Caretaker");
        }


        [HttpGet]
        public IEnumerable<Caretaker> Get() => _caretakerCollection.AsQueryable<Caretaker>().ToEnumerable();

        [HttpGet("{id}")]
        public ActionResult<Caretaker> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Value must be passed in the request body");
            }
            return Ok(_caretakerCollection.AsQueryable<Caretaker>().SingleOrDefault(x => x.ctID == id));
        }
    }
}
