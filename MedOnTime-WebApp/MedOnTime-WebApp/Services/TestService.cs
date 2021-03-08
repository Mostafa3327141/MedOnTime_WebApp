using MedOnTime_WebApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Services
{
    public class TestService
    {
        private readonly IMongoCollection<Test> _Tests;

        public TestService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _Tests = database.GetCollection<Test>("Tests");
        }

        public Test Create(Test Test)
        {
            _Tests.InsertOne(Test);
            return Test;
        }

        public IList<Test> Read() =>
            _Tests.Find(sub => true).ToList();

        public Test Find(string id) =>
            _Tests.Find(sub => sub.Id == id).SingleOrDefault();

        public void Update(Test Test) =>
            _Tests.ReplaceOne(sub => sub.Id == Test.Id, Test);

        public void Delete(string id) =>
            _Tests.DeleteOne(sub => sub.Id == id);
    }
}
