using MongoDB.Driver;
using System;

namespace Sparrow.Stores.Mongo
{
    public class MongoAccessor : IMongoAccessor
    {
        private IMongoClient _mongoClient;
        private IMongoDatabase _defaultDatabase;

        public MongoSettings Settings { get; protected set; } = new MongoSettings();

        public MongoAccessor(Action<MongoSettings> action)
        {
            action?.Invoke(Settings);
        }

        public IMongoClient MongoClient => _mongoClient ?? (_mongoClient = new MongoClient(Settings.ClientSettings));
        
        public IMongoDatabase DefaultDatabase => _defaultDatabase ?? (_defaultDatabase = GetDatabase());

        public IMongoDatabase GetDatabase(string database = null, MongoDatabaseSettings settings = null)
        {
            if (string.IsNullOrEmpty(database))
                database = Settings.DatabaseName;

            if (settings == null)
                settings = Settings.DatabaseSettings;

            return MongoClient.GetDatabase(database, settings);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string collection, MongoCollectionSettings settings = null)
        {
            if (settings == null)
                settings = Settings.CollectionSettings;

            return DefaultDatabase.GetCollection<TDocument>(collection, settings);
        }
    }
}
