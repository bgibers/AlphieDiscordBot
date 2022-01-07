using System.Collections.Generic;
using System.Threading.Tasks;
using AlphieDiscordBot.DataAccess.Models;

namespace AlphieDiscordBot.DataAccess.Repositories
{
    public interface IAlphieMintsRepository
    {
        /// <summary>
        /// Get all documents stored
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AlphieMintProject>> GetAllMintProjects();  
        
        /// <summary>
        /// Get a project by its stored ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AlphieMintProject> GetMintProject(long id);
        
        /// <summary>
        /// Get a project by its name
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        Task<AlphieMintProject> GetMintProjectByName(string projectName);

        /// <summary>
        /// Get all mints by mint status
        /// </summary>
        /// <param name="alreadyMinted"></param>
        /// <returns></returns>
        Task<List<AlphieMintProject>> GetAllMintsByStatus(bool alreadyMinted);

        /// <summary>
        /// Checks if a project is already existent 
        /// </summary>
        /// <param name="mintProject"></param>
        /// <returns></returns>
        Task<bool> Exists(AlphieMintProject mintProject);
        
        /// <summary>
        /// Create a new document
        /// </summary>
        /// <param name="mintProject"></param>
        /// <returns></returns>
        Task Create(AlphieMintProject mintProject);        
        
        /// <summary>
        /// Update the document
        /// </summary>
        /// <param name="mintProject"></param>
        /// <returns></returns>
        Task<bool> Update(AlphieMintProject mintProject);
        
        /// <summary>
        /// Delete record from document
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        Task<bool> Delete(string projectName);

        /// <summary>
        /// Delete all records from database
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteEntireCollection();
        
        /// <summary>
        /// Next available ID
        /// </summary>
        /// <returns></returns>
        Task<long> GetNextId();
    }
}