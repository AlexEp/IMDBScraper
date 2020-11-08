using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMDBScraper.DAL.MongoRepository;
using IMDBScraper.DAL.Repository;
using IMDBScraper.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IMDBScraper.Worker
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
                    IConfiguration configuration = hostContext.Configuration;

                    /* *** [Logger settings] *** */
                    var loggerFactory = LoggerFactory.Create(builder =>
                    {
                        builder
                            .AddConfiguration(configuration.GetSection("Logging"))
                            .AddConsole();
                    });

                    // *** init MongoDB *** */
                    var mongoSection = configuration.GetSection("MongoDB");
                    var mongoDBConfig = new MongoDBConfig();
                    mongoSection.Bind(mongoDBConfig);

                    services.AddTransient<IActorRepository>(provider => new ActorRepository(mongoDBConfig));


                    // *** init IMDBScraperService ***  */
                    var WorkerConfig = configuration.GetSection("WorkerConfig");
                    var baseURL = WorkerConfig["BaseURL"];
                    var actorsURL = WorkerConfig["ActorsURL"];

                    var taskEnginelogger = loggerFactory.CreateLogger<IMDBScraperService>();

                    services.AddTransient<IIMDBScraperService>(provider => new IMDBScraperService(baseURL, actorsURL, taskEnginelogger));

     
                    services.AddHostedService<Worker>();
                });
    }
}
