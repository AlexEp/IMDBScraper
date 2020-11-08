using IMDBScraper.Shared.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IMDBScraper.Worker.Services
{
    public interface IIMDBScraperService
    {
        Task<IEnumerable<Actor>> GetActorsAsync(CancellationToken cancellationToken);
    }
}
