using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using IMDBScraper.DAL.Repository;
using IMDBScraper.Shared.DTO;
using IMDBScraper.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IMDBScraper.Worker.Services
{
    public class IMDBScraperService  : WebScraperService, IIMDBScraperService
    {
        private readonly string _baseURL;
        private readonly string _actorsURL;
        private readonly int _maxHTTPCalled = 10;
        private readonly ILogger<IMDBScraperService> _logger;


        public IMDBScraperService(string baseURL, string actorsURL, ILogger<IMDBScraperService> logger, int maxHTTPCalled = 10)
        {
            _baseURL = baseURL;
            _actorsURL = actorsURL;
            _maxHTTPCalled = maxHTTPCalled;
            _logger = logger;
        }

        private object syncObject = new object();
        public async Task<IEnumerable<Actor>> GetActorsAsync(CancellationToken cancellationToken)
        {
            var document = await ScrapeWebsiteAsync($"{_baseURL}{_actorsURL}", cancellationToken).ConfigureAwait(false);
            var articleLink = document.All.First(x => x.ClassName == "lister-list");
            var actorsNodes = articleLink.QuerySelectorAll(".lister-item.mode-detail").ToList();

            //////////////////////////////
            //Find the URLs of the actors

            IList<string> urlsList = new List<string>(actorsNodes.Count);

            foreach (var actor in actorsNodes)
            {
                var url = actor.QuerySelectorAll(".lister-item-header > a").FirstOrDefault()?.Attributes["href"].Value;
                string fullUrl = $"{_baseURL}{url}";
                urlsList.Add(fullUrl);
            }

            //////////////////////////////
            //Read any actor URL

            SemaphoreSlim semaphoreObject = new SemaphoreSlim(_maxHTTPCalled);
            IList<Task<Actor>> tasksList = new List<Task<Actor>>(actorsNodes.Count);
            ConcurrentBag<Actor> actorsList = new ConcurrentBag<Actor>();

            foreach (var url in urlsList)
            {
                await semaphoreObject.WaitAsync();
                var t = Task.Run<Actor>(async () =>
                {
                    try
                    {
                        var actor = await ProcessActorAsync(url, cancellationToken).ConfigureAwait(false);
                        return actor;
                    }
                    //catch (OperationCanceledException canceled)
                    //{
                    //    _logger.LogWarning(canceled.Message);
                    //}
                    //catch (Exception ex)
                    //{
                    //    _logger.LogError($"Faild to read {url}", ex.Message);
                    //}
                    finally
                    {
                        semaphoreObject.Release();
                    }
                });
                tasksList.Add(t);
            }



            while (tasksList.Count > 0)
            {
                var task = await Task.WhenAny<Actor>(tasksList);
                tasksList.Remove(task);
                try
                {
                    var actor = await task;
                    _logger.LogDebug($"Successfully read {actor.Name}");
                    actorsList.Add(actor);
                }
                catch (OperationCanceledException) { }
                catch (Exception exc) { _logger.LogError($"Faild to read", exc.Message); }
            }

    
            return actorsList;
        }


        private async Task<Actor> ProcessActorAsync(string siteUrl, CancellationToken cancellationToken) {

            Actor actor = new Actor();

        
                var nodes = await ScrapeActorAsync(siteUrl, cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                var name = HTMPHelper.KeepOnlyLetters(nodes.QuerySelectorAll(".name-overview-widget__section .header .itemprop").FirstOrDefault()?.InnerHtml);
                var imgURL = nodes.QuerySelectorAll(".poster-hero-container .image #name-poster").FirstOrDefault()?.Attributes["src"].Value;
                var bornDateStr = nodes.QuerySelectorAll("#name-born-info time[datetime]").FirstOrDefault()?.Attributes["datetime"].Value;

                var jobCategories = nodes.QuerySelectorAll("#name-job-categories > a").Select(c => HTMPHelper.KeepOnlyLetters(c.TextContent));

                actor.Name = name;
                actor.Roles = jobCategories;
                actor.ImageURL = imgURL;

                //Gender
                if (actor.Roles.Any(r => r.Trim().ToLower() == "Actress".Trim().ToLower()))
                {
                    actor.Gender = Shared.ActorGender.Female;
                }
                else if (actor.Roles.Any(r => r.Trim().ToLower() == "Actor".Trim().ToLower()))
                {
                    actor.Gender = Shared.ActorGender.Male;
                }

                //Birthday
                DateTime birthday = DateTime.MinValue;

                if (DateTime.TryParseExact(bornDateStr, "yyyy-M-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out birthday))
                {
                    actor.Birthday = birthday;
                }
                else if (DateTime.TryParseExact(bornDateStr, "yyyy-MM-dd",
                  CultureInfo.InvariantCulture,
                  DateTimeStyles.None, out birthday))
                {
                    actor.Birthday = birthday;
                }
                else if (DateTime.TryParseExact(bornDateStr, "yyyy-MM-d",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out birthday))
                {
                    actor.Birthday = birthday;
                }
                else if (DateTime.TryParseExact(bornDateStr, "yyyy-M-d",
                      CultureInfo.InvariantCulture,
                      DateTimeStyles.None, out birthday))
                {
                    actor.Birthday = birthday;
                }

            return actor;
        }


        private async Task<IHtmlDocument> ScrapeActorAsync(string siteUrl, CancellationToken cancellationToken)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(siteUrl).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync().ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            //Parse
            HtmlParser parser = new HtmlParser();
            var document = parser.ParseDocument(response);
            return document;
        }

        private static class HTMPHelper {

            public static string KeepOnlyLetters(string str) {
                var onlyLetters = new String(str.Where(c => Char.IsLetter(c) || Char.IsWhiteSpace(c)).ToArray());
                return onlyLetters;
            }
        }
    }
}
