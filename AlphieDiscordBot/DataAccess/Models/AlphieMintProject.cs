using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AlphieDiscordBot.DataAccess.Models
{
    public class AlphieMintProject
    {
        /// <summary>
        /// Internal UUID
        /// </summary>
        [BsonId]
        public ObjectId InternalId { get; set; }
        
        /// <summary>
        /// Primary key id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// The name of the project we're setting reminders for
        /// </summary>
        public string ProjectName { get; set; } 
        
        /// <summary>
        /// The official URL of the projects website
        /// </summary>
        public string ProjectUrl { get; set; }
        
        /// <summary>
        /// Official twitter url of the project
        /// </summary>
        public string TwitterUrl { get; set; }
        
        /// <summary>
        /// Flips to true once mint date has passed
        /// </summary>
        public bool AlreadyMinted { get; set; }
        
        /// <summary>
        /// The date this mint takes place
        /// </summary>
        public DateTimeOffset MintDate { get; set; }
        
        /// <summary>
        /// The time the next reminder should be sent 
        /// </summary>
        public DateTimeOffset? NextReminder { get; set; }
    }
}