using AlphieDiscordBot.Config;
using AlphieDiscordBot.DataAccess.Context;
using AlphieDiscordBot.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AlphieDiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var mongoDbConfig = new MongoDbConfig();
                    hostContext.Configuration.GetSection("MongoDB").Bind(mongoDbConfig);
                    
                    var alphieContext = new AlphieContext(mongoDbConfig);
                    
                    services.AddSingleton<IAlphieMintsRepository>(new AlphieMintsRepository(alphieContext));
                    services.AddHostedService<Worker>();
                });
    }
}