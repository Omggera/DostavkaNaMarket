using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

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

        // Получение всех документов в коллекции
        public void FindAllDocs(List<BsonDocument> list)
        {
            var filter = new BsonDocument();
            var people =  Collection.Find(filter).ToList();
            foreach (var doc in people)
            {
                list.Add(doc);
            }
        }

        FilterDefinition<BsonDocument> filterAnd;

        public void GlobalFilter(List<BsonDocument> list, string? startDate, string? phone, string? order,
                                 string? email, string? getMethod, string? payMethod )
        {
            //var builder = Builders<BsonDocument>.Filter;
            var filterDate = Builders<BsonDocument>.Filter.Eq("dateDevivery", $"{startDate}");
            var filterPhone = Builders<BsonDocument>.Filter.Regex("clientPhone", new BsonRegularExpression($"{phone}"));
            var filterOrder = Builders<BsonDocument>.Filter.Eq("orderNum", $"{order}");
            var filterEmail = Builders<BsonDocument>.Filter.Regex("clientMail", new BsonRegularExpression($"{email}"));
            var filterGetMethod = Builders<BsonDocument>.Filter.Eq("getMethod", $"{getMethod}");
            var filterPayMethod = Builders<BsonDocument>.Filter.Eq("paymentMethod", $"{payMethod}");

            if (startDate == null) filterDate = Builders<BsonDocument>.Filter.Ne("dateDevivery", $"{startDate}");
            if (phone == null) filterPhone = Builders<BsonDocument>.Filter.Ne("clientPhone", $"{phone}");
            if (order == null) filterOrder = Builders<BsonDocument>.Filter.Ne("orderNum", $"{order}");
            if ((email == null) & (email == "")) filterEmail = Builders<BsonDocument>.Filter.Ne("clientMail", $"{email}");
            if (getMethod == "Все") filterGetMethod = Builders<BsonDocument>.Filter.Ne("getMethod", $"{getMethod}");
            if (payMethod == "Все") filterPayMethod = Builders<BsonDocument>.Filter.Ne("paymentMethod", $"{payMethod}");

            filterAnd = Builders<BsonDocument>.Filter.And(new List<FilterDefinition<BsonDocument>> 
            { 
                filterDate,
                filterPhone,
                filterOrder,
                filterEmail,
                filterPayMethod, 
                filterGetMethod
            });
            
            var people = Collection.Find(filterAnd).ToList();
            foreach (var doc in people)
            {
                list.Add(doc);
            }

        }

        public string? ModalWorks(string? order, string field, List<string> list)
        {
            var filter = new BsonDocument("orderNum", $"{order}");
            var user = Collection.Find(filter).FirstOrDefault();

            var dictionary = user.ToDictionary();
            
            var arrayOfStrings = user["barcodes"].AsBsonArray.Select(p => p.AsString).ToArray();

            string? dataValue;
            if(field == "barcodes")
            {
                list.Clear();
                foreach (var i in arrayOfStrings)
                {
                    list.Add(i);
                }
                return null;
            }
            else
            {
                dataValue = dictionary[field].ToString();
            }
            return dataValue;
        }

        public void UpdateDocument(string? order, string? city, string? name, string? phone, string? mail, string? getMethod, 
                                   string? adress, string? payMethod, int? amount, List<string> barcodes)
        {
            Collection.UpdateOneAsync(
                new BsonDocument("orderNum", $"{order}"),
                new BsonDocument("$set", new BsonDocument 
                {
                    {"city",$"{city}"},
                    {"clientName",$"{name}"},
                    {"clientPhone",$"{phone}"},
                    {"clientMail",$"{mail}"},
                    {"getMethod",$"{getMethod}"},
                    {"clientAdress",$"{adress}"},
                    {"paymentMethod",$"{payMethod}"},
                    {"finalAmount",$"{amount}"},
                    {"barcodes", new BsonArray(barcodes)}
                }
                ));
        }
    }
}
