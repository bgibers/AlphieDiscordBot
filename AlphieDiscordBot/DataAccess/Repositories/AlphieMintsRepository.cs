using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphieDiscordBot.DataAccess.Context;
using AlphieDiscordBot.DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlphieDiscordBot.DataAccess.Repositories
{
    public class AlphieMintsRepository : IAlphieMintsRepository
    {
        private readonly IAlphieContext _context;

        public AlphieMintsRepository(IAlphieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AlphieMintProject>> GetAllMintProjects()
        {
            return await _context
                .AlphieProjects
                .Find(_ => true)
                .ToListAsync();
        }

        public async Task<AlphieMintProject> GetMintProject(long id)
        {
            FilterDefinition<AlphieMintProject> filter = Builders<AlphieMintProject>.Filter.Eq(m => m.Id, id);
            return await _context
                .AlphieProjects
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<AlphieMintProject> GetMintProjectByName(string projectName)
        {
            FilterDefinition<AlphieMintProject> filter = Builders<AlphieMintProject>.Filter.Eq(m => m.ProjectName, projectName);
            return await _context
                .AlphieProjects
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AlphieMintProject>> GetAllMintsByStatus(bool alreadyMinted)
        {
            FilterDefinition<AlphieMintProject> filter = Builders<AlphieMintProject>.Filter.Eq(m => m.AlreadyMinted, alreadyMinted);
            return await _context
                .AlphieProjects
                .Find(filter).ToListAsync();
        }

        public async Task<bool> Exists(AlphieMintProject mintProject)
        {
            var filter = Builders<AlphieMintProject>.Filter.Or(
                Builders<AlphieMintProject>.Filter.Eq(m => m.ProjectName, mintProject.ProjectName),
                Builders<AlphieMintProject>.Filter.Eq(m => m.ProjectUrl, mintProject.ProjectUrl),
                Builders<AlphieMintProject>.Filter.Eq(m => m.TwitterUrl, mintProject.TwitterUrl)
            );
            
            return await _context
                .AlphieProjects
                .Find(filter).AnyAsync();
        }
        
        public async Task Create(AlphieMintProject mintProject)
        {
            await _context.AlphieProjects.InsertOneAsync(mintProject);
        }

        public async Task<bool> Update(AlphieMintProject mintProject)
        {
            ReplaceOneResult updateResult =
                await _context
                    .AlphieProjects
                    .ReplaceOneAsync(
                        filter: g => g.Id == mintProject.Id,
                        replacement: mintProject);
            
            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string projectName)
        {
            FilterDefinition<AlphieMintProject> filter = Builders<AlphieMintProject>.Filter.Eq(m => m.ProjectName, projectName);
            DeleteResult deleteResult = await _context
                .AlphieProjects
                .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteEntireCollection()
        {
            var result = await _context
                .AlphieProjects.DeleteManyAsync(Builders<AlphieMintProject>.Filter.Empty);

            return true;
        }

        public async Task<long> GetNextId()
        {
            try
            {
                return await _context.AlphieProjects.CountDocumentsAsync(new BsonDocument()) + 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}