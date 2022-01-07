using System;
using System.Threading.Tasks;
using AlphieDiscordBot.DataAccess.Models;
using AlphieDiscordBot.DataAccess.Repositories;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace AlphieDiscordBot.Commands
{
    public class MintAlertModule : BaseCommandModule
    {
        public IAlphieMintsRepository _mintsRepository { private get; set; }
        
        /// <summary>
        /// Add a mint to the notifications list
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="projectName">Some project name</param>
        /// <param name="url">https://someURL.com</param>
        /// <param name="twitterUrl">https://twitter.com/someprofile</param>
        /// <param name="mintDate">01/01/21 10:30PM (NOTE THIS MUST BE IN EST)</param>
        /// <returns></returns>
        [Command("alphieAdd"), DescriptionAttribute("Add reminders for an upcoming mint")]
        public async Task StartMonitoringMint(CommandContext ctx, 
            [Description("\nName of the project")]string projectName,
            [Description("\nUrl of the project")]string url, 
            [Description("\nOfficial twitter url of the project")]string twitterUrl, 
            [Description("\nDate and time of the mint.\n In eastern standard time. \n ex. 01/01/22 8:30AM")]string mintDate)
        {
            
            try
            {
                var dateTime = DateTime.Parse(mintDate);
                var mintDateOffset = new DateTimeOffset(dateTime);
                var alphieProject = new AlphieMintProject()
                {
                    Id = await _mintsRepository.GetNextId(),
                    ProjectName = projectName,
                    ProjectUrl = url,
                    TwitterUrl = twitterUrl,
                    AlreadyMinted = false,
                    MintDate = mintDateOffset
                };

                if (await _mintsRepository.Exists(alphieProject))
                {
                    await ctx.RespondAsync($"I'm already watching this project. Type !upcomingMints to see all projects");
                    return;
                }

                await _mintsRepository.Create(alphieProject);
                await ctx.RespondAsync($"Will start reminders for **{projectName}**\n\n Project URL: <{url}>" +
                                       $" \n\n Twitter: {twitterUrl} \n\n Mint date : <t:{mintDateOffset.ToUnixTimeSeconds()}:F>");
            }
            catch (Exception e)
            {
                await ctx.RespondAsync($"Invalid input. Date should look like the following 10/30/21 10:30PM. Remember in EST");
            }
        }
        
        [Command("alphieUpcoming"),DescriptionAttribute("View all upcoming mints")]
        public async Task ViewAllUpcomingMints(CommandContext ctx)
        {
            var upcomingMints = await _mintsRepository.GetAllMintsByStatus(false);
            var responseString = "";
            foreach (var mint in upcomingMints)
            {
                responseString += $"**{mint.ProjectName}**\n\n Project URL: <{mint.ProjectUrl}>" +
                                  $" \n\n Twitter: <{mint.TwitterUrl}> \n\n Mint date : <t:{mint.MintDate.ToUnixTimeSeconds()}:F> \n\n\n";
            }

            if (string.IsNullOrEmpty(responseString))
            {
                responseString = "No projects have been scheduled yet";
            }
            
            await ctx.RespondAsync(responseString);
        }
        
        [Command("alphieUpdate"), DescriptionAttribute("Update info on a specified mint. Authorized users only.")]
        public async Task UpdateMint(CommandContext ctx, 
            [Description("\nName of the project")]string projectName,
            [Description("\nUrl of the project")]string url, 
            [Description("\nOfficial twitter url of the project")]string twitterUrl, 
            [Description("\nDate and time of the mint.\n In eastern standard time. \n ex. 01/01/22 8:30AM")]string mintDate)
        {
            
        }
        
        [Command("alphieDelete"), DescriptionAttribute("Delete reminders for a mint. Authorized users only.")]
        public async Task DeleteMint(CommandContext ctx, [DescriptionAttribute("\nProject to delete. Authorized users only.")]string projectName)
        {
            try
            {
                await _mintsRepository.Delete(projectName);
                await ctx.RespondAsync($"Deleted project with name {projectName} from reminders.");
            }
            catch (Exception e)
            {
                await ctx.RespondAsync($"Project {projectName} was not found.");
            }
        }
    }
}