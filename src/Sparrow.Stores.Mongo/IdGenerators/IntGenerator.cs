using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;

namespace Sparrow.Stores.Mongo.IdGenerators
{
    public class IntGenerator : IIdGenerator
    {
        public static IntGenerator Instance { get; } = new IntGenerator();

        public object GenerateId(object container, object document)
        {
            var dbObject = container
                .GetType()
                .GetProperty("Database")
                .GetValue(container);

            if (dbObject is IMongoDatabase database)
            {
                var sortDefBuilder = new SortDefinitionBuilder<BsonDocument>();

                var doc = database
                    .GetCollection<BsonDocument>(document.GetType().Name)
                    .Find(s => true)
                    .Sort(sortDefBuilder.Descending("_id"))
                    .FirstOrDefault();

                return doc == null ? 1 : doc.GetValue("_id").ToInt32() + 1;
            }
            return null;
        }

        public bool IsEmpty(object id)
        {
            return id == null || (int)id == default;
        }
    }
}

