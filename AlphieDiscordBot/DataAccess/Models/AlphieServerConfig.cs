using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlphieDiscordBot.DataAccess.Models
{
    public class AlphieServerConfig
    {
        /// <summary>
        /// Internal UUID
        /// </summary>
        [BsonId]
        public ObjectId InternalId { get; set; }

    }
}