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

        public async Task AddDoc(BsonDocument doc)
        {
            await Collection.InsertOneAsync(doc);
        }

        public long AllDocCount()
        {
            var filter = new BsonDocument();
            long count = Collection.CountDocuments(filter);
            return count;
        }
    }
}
