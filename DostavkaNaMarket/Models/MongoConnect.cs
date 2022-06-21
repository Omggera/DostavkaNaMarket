using MongoDB.Bson;
using MongoDB.Driver;

namespace DostavkaNaMarket.Models
{
    public class MongoConnect
    {
        string connectionString = "mongodb://localhost:27017";

        public void Connect()
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("test");
        }
        
    }
}
