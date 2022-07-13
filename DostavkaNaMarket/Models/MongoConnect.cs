using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace DostavkaNaMarket.Models
{
    public class MongoConnect
    {
        IMongoDatabase database; // база данных

        public MongoConnect()
        {
            // строка подключения
            string connectionString = "mongodb://localhost:27017";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
        }
        // обращаемся к коллекции users
        public IMongoCollection<User> Users
        {
            get { return database.GetCollection<User>("users"); }
        }
        // получаем все документы, используя критерии фильтрации
        public async Task<IEnumerable<User>> GetUsers(string phone, string name)
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            // фильтр по имени
            if (!String.IsNullOrWhiteSpace(name))
            {
                filter = filter & builder.Regex("clientName", new BsonRegularExpression(name));
            }
            // фильтр по номеру телефона
            if (!String.IsNullOrWhiteSpace(phone))
            {
                filter = filter & builder.Regex("clientPhone", new BsonRegularExpression(phone));
            }

            return await Users.Find(filter).ToListAsync();
        }

        // получаем один документ по id
        public async Task<User> GetUserId(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        // добавление документа
        public async Task Create(User c)
        {
            await Users.InsertOneAsync(c);
        }
        // обновление документа 
        //public async Task Update(User c)
        //{
        //    await Users.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(c.Id)), c);
        //}
        // удаление документа
        public async Task Remove(string id)
        {
            await Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}
