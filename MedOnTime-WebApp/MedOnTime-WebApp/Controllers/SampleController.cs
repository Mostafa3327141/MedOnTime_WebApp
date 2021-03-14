using MedOnTime_WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Bson;

namespace MedOnTime_WebApp.Controllers
{
    public class SampleController : Controller
    {
        private IMongoCollection<Sample> _sampleCollection;

        public MongoClientSettings ConfigurationManager { get; }

        public SampleController(IMongoClient client)
        {
            var database = client.GetDatabase("MedOnTimeDb");
            _sampleCollection = database.GetCollection<Sample>("Sample");
        }

        // GET: SampleController1
        public ActionResult Index()
        {
            List<Sample> samples = _sampleCollection.AsQueryable<Sample>().ToList();
            return View(samples);
        }

        // GET: SampleController1/Details/5
        public ActionResult Details(string id)
        {
            var sampleId = new ObjectId(id);
            var sample = _sampleCollection.AsQueryable<Sample>().SingleOrDefault(x => x.Id == sampleId);
            return View(sample);
        }

        // GET: SampleController1/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: SampleController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sample sample)
        {
            try
            {
                _sampleCollection.InsertOne(sample);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SampleController1/Edit/5
        public ActionResult Edit(string id)
        {
            var sampleId = new ObjectId(id);
            var sample = _sampleCollection.AsQueryable<Sample>().SingleOrDefault(x => x.Id == sampleId);
            return View(sample);
        }

        // POST: SampleController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Sample sample)
        {
            try
            {
                var filter = Builders<Sample>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<Sample>.Update
                    .Set("SampleName", sample.SampleName)
                    .Set("SampleDescription", sample.SampleDescription)
                    .Set("SampleQuantity", sample.SampleQuantity);
                var result = _sampleCollection.UpdateOne(filter, update);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SampleController1/Delete/5
        public ActionResult Delete(string id)
        {
            var sampleId = new ObjectId(id);
            var sample = _sampleCollection.AsQueryable<Sample>().SingleOrDefault(x => x.Id == sampleId);
            return View(sample);
        }

        // POST: SampleController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                _sampleCollection.DeleteOne(Builders<Sample>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
