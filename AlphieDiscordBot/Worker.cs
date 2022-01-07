using System;
using System.Threading;
using System.Threading.Tasks;
using AlphieDiscordBot.Commands;
using AlphieDiscordBot.Config;
using AlphieDiscordBot.DataAccess.Context;
using AlphieDiscordBot.DataAccess.Repositories;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlphieDiscordBot
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> _logger;
        private IConfiguration _configuration;
        private DiscordClient _discordClient;
        
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) =>  Task.CompletedTask;

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting discord bot");

            string discordBotToken = _configuration["DiscordBotToken"];
            _discordClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = discordBotToken,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = LogLevel.Debug
            });
            
            
            // wire up mongo to work with commands
            var mongoDbConfig = new MongoDbConfig();
            _configuration.GetSection("MongoDB").Bind(mongoDbConfig);
            var alphieContext = new AlphieContext(mongoDbConfig);
                    
            var services = new ServiceCollection()
                .AddSingleton<IAlphieMintsRepository>(new AlphieMintsRepository(alphieContext))
                .AddLogging()
                .BuildServiceProvider();
            
            var commands = _discordClient.UseCommandsNext(new CommandsNextConfiguration()
            { 
                Services = services,
                StringPrefixes = new[] { "!" },
                EnableDefaultHelp = true
            });
            
            _discordClient.MessageCreated += OnMessageCreated;
            commands.RegisterCommands<MintAlertModule>();
            await _discordClient.ConnectAsync();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _discordClient.MessageCreated -= OnMessageCreated;
            await _discordClient.DisconnectAsync();
            _discordClient.Dispose();
            _logger.LogInformation("Discord bot stopped");
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Message.Content.Equals("Alphie", StringComparison.OrdinalIgnoreCase))
            {
                await e.Message.RespondAsync("Type !alphieHelp to see my list of commands");
            }
        }
    }
}