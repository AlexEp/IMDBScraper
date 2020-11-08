using IMDBScraper.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMDBScraper.DAL.Repository
{
    public interface IActorRepository : IRepository<IMDBScraper.Shared.DTO.Actor>
    {
        Task<IEnumerable<IMDBScraper.Shared.DTO.Actor>> GetAllAsync();
        Task<IEnumerable<IMDBScraper.Shared.DTO.Actor>> GetAllUnHidedAsync();
        Task<bool> HideAsync(string id);
        Task<bool> UnHideAllAsync();
    }
}
