using AlphieDiscordBot.DataAccess.Models;
using MongoDB.Driver;

namespace AlphieDiscordBot.DataAccess.Context
{
    public interface IAlphieContext
    {
        /// <summary>
        /// All projects that are under the mint watch
        /// </summary>
        IMongoCollection<AlphieMintProject> AlphieProjects { get; }
        
        // IMongoCollection<AlphieServerConfig> AlphieConfig { get; }

    }
}