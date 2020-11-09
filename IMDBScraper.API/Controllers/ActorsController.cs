using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IMDBScraper.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IMDBScraper.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly ILogger<ActorsController> _logger;
        private readonly IActorRepository _actorRepository;

        public ActorsController(ILogger<ActorsController> logger, IActorRepository actorRepository)
        {
            _logger = logger;
            _actorRepository = actorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<IMDBScraper.Shared.DTO.Actor> actors = new List<IMDBScraper.Shared.DTO.Actor>(0);
            try
            {
                 actors = await _actorRepository.GetAllUnHidedAsync().ConfigureAwait(false);
            }
            catch (System.Exception exp)
            {
                _logger.LogError("Error: Get Actors. {@exeption}", exp.Message);
            }

            return Ok(actors);
        }

        [HttpDelete("{actorID}")]
        public async Task<IActionResult> Delete([FromRoute]string actorID)
        {
            try
            {
                var actors = await _actorRepository.HideAsync(actorID);
            }
            catch (System.Exception exp)
            {
                _logger.LogError("Error: Hide actorID: {@actorID}. {@exeption}", actorID, exp.Message);
                return BadRequest();
            }
            return new JsonResult(actorID);
        }

        [HttpPost("unhideall")]
        public async Task<IActionResult> UnHideAll()
        {
            try
            {
                var actors = await _actorRepository.UnHideAllAsync();
            }
            catch (System.Exception exp)
            {
                _logger.LogError("Error, UnHideAllAsync: {@exeption}.", exp.Message);
                return BadRequest();
            }
            return Ok();
        }
    }
}
