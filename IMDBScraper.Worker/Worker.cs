using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using IMDBScraper.DAL.Repository;
using IMDBScraper.Shared.DTO;
using IMDBScraper.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IMDBScraper.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IIMDBScraperService _IMDBScraperService;
        private readonly IActorRepository _actorRepository;
        public Worker(ILogger<Worker> logger, 
            IConfiguration configuration, 
            IIMDBScraperService IMDBScraperService, IActorRepository actorRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _IMDBScraperService = IMDBScraperService;
            _actorRepository = actorRepository;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var actorsFromWeb = await _IMDBScraperService.GetActorsAsync(stoppingToken).ConfigureAwait(false); ;
                    var actorsFromDB = await _actorRepository.GetAllAsync().ConfigureAwait(false);
                    
                    IList<Actor> actorToInsert = new List<Actor>(actorsFromWeb.Count());
                    IList<Actor> actorToUpdate = new List<Actor>(actorsFromDB.Count());
                    IList<Actor> actorToDelete = new List<Actor>(actorsFromDB.Count());

                    foreach (var actor in actorsFromWeb)
                    {
                        if (actorsFromDB.Any(a => a.Name.Trim().ToLower() == actor.Name.Trim().ToLower()))
                        {
                            actorToUpdate.Add(actor);
                        }
                        else
                        {
                            actorToInsert.Add(actor);
                        }
                    }

                    foreach (var actor in actorsFromDB)
                    {
                        if (!actorsFromWeb.Any(a => a.Name.Trim().ToLower() == actor.Name.Trim().ToLower()))
                        {
                            actorToDelete.Add(actor);
                        }
                    }

                    //Insert Actor
                    foreach (var actor in actorToInsert)
                    {
                        try
                        {
                            await _actorRepository.InsertAsync(actor);
                            _logger.LogInformation($"Actor: '{actor.Name}'  Added");
                        }
                        catch (Exception exp)
                        {
                            _logger.LogError($"Failed to add actor: '{actor.Name}'", exp);
                        }
         
                    }

                    //Update Actor
                    foreach (var actor in actorToUpdate)
                    {
                        try
                        {
                            await _actorRepository.UpdateAsync(actor);
                            _logger.LogDebug($"Actor: '{actor.Name}'  Updated");
                        }
                        catch (Exception exp)
                        {
                            _logger.LogError($"Failed to update actor: '{actor.Name}'", exp);
                        }
                    }

                    //Delete Actor
                    foreach (var actor in actorToDelete)
                    {
                        try
                        {
                            await _actorRepository.DeleteAsync(actor);
                            _logger.LogInformation($"Actor: '{actor.Name}'  Deleted");
                        }
                        catch (Exception exp)
                        {
                            _logger.LogError($"Failed to delete actor: '{actor.Name}'", exp);
                        }
                    }

                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex) { ErrorHandle(ex); }

           
            }

            return;
        }

        private void ErrorHandle(Exception ex)
        {
            _logger.LogError(ex.Message, ex.ToString());
        }
    }
}
