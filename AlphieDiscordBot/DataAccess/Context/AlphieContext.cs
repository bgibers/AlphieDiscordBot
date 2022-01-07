using AlphieDiscordBot.Config;
using AlphieDiscordBot.DataAccess.Models;
using MongoDB.Driver;

namespace AlphieDiscordBot.DataAccess.Context
{
    public class AlphieContext : IAlphieContext
    {
        private readonly IMongoDatabase _db; 
        
        public AlphieContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);
        }        
        
        public IMongoCollection<AlphieMintProject> AlphieProjects => _db.GetCollection<AlphieMintProject>("AlphieProjects");
    }
}