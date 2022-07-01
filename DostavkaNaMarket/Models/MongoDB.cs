﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace DostavkaNaMarket.Models
{
    public class MongoDB
    {
        IMongoDatabase database;
        public MongoDB()
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            database = client.GetDatabase("test");
        }

        public IMongoCollection<BsonDocument> Collection
        {
            get { return database.GetCollection<BsonDocument>("users"); }
        }

        // Добавить документ
        public async Task AddDoc(BsonDocument doc)
        {
            await Collection.InsertOneAsync(doc);
        }

        // Получить общее число документов в коллекции
        public long AllDocCount()
        {
            var filter = new BsonDocument();
            long count = Collection.CountDocuments(filter);
            return count;
        }

        public string DataForPreview(string field)
        {
            var filter = new BsonDocument("orderNum", $"{AllDocCount()}");
            var user = Collection.Find(filter).FirstOrDefault();
            var dictionary = user.ToDictionary();
            string? dataForPrev = dictionary[field].ToString();
            return dataForPrev;
        }

        public void FindAllDocs(List<BsonDocument> list)
        {
            var filter = new BsonDocument();
            var people =  Collection.Find(filter).ToList();
            foreach (var doc in people)
            {
                list.Add(doc);
            }
        }

        public void FindAllDateDocs(List<BsonDocument> list, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gte("dateOrder", $"{startDate}") & builder.Lte("dateOrder", $"{endDate}");
            //var filter = new BsonDocument("$and", new BsonArray{

            //    new BsonDocument("dateOrder", new BsonDocument("$gte", $"{startDate}")),
            //    new BsonDocument("dateOrder", new BsonDocument("$lte", $"{endDate}"))
            //});
            var people = Collection.Find(filter).ToList();
            if (people.Count > 0)
            {
                foreach (var doc in people)
                {
                    list.Add(doc);
                }
            }
            else
            {
                list.Clear();
            }
            
        }
    }
}
