using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Sparrow.Stores.Mongo.IdGenerators
{
    public class LongGenerator : IIdGenerator
    {
        public static LongGenerator Instance { get; } = new LongGenerator();

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

                return doc == null ? 1 : doc.GetValue("_id").ToInt64() + 1;
            }
            return null;
        }

        public bool IsEmpty(object id)
        {
            return id == null || (long)id == default;
        }
    }
}
