using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IMDBScraper.Worker.Services
{
    public class WebScraperService
    {
        internal async Task<IHtmlDocument> ScrapeWebsiteAsync(string siteUrl, CancellationToken cancellationToken)
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
    }
}
