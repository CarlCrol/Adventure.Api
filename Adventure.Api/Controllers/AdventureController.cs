using Adventure.Core;
using Adventure.Core.Administration.Commands;
using Adventure.Core.Administration.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdventureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdventureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Adventures
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="maxItems"></param>
        /// <response code="200">Returns the Adventures</response>
        [HttpGet("{currentPage}/{maxItems}")]
        [ProducesResponseType(typeof(ServiceResponse<GetAdventuresQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int currentPage, int maxItems)
        {
            return Ok(await _mediator.Send(new GetAdventuresQuery(currentPage, maxItems)));
        }

        /// <summary>
        /// Gets Adventure By its Identifier
        /// </summary>
        /// <param name="adventureId"></param>
        /// <response code="200">Returns the Adventure</response>
        /// <response code="400">If Adventure does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ServiceResponse<Dto.AdventureReadModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int adventureId)
        {
            return Ok(await _mediator.Send(new GetAdventureQuery(adventureId)));
        }

        /// <summary>
        /// Adds new Adventure
        /// </summary>
        /// <param name="adventure"></param>
        /// <response code="201">Returns the identifier of the new Adventure</response>
        /// <response code="400">if new adventure has invalid properties</response>
        /// <response code="409">if new adventure has conflict to existing adventures</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Add([FromBody] Dto.Adventure adventure)
        {
            var response = await _mediator.Send(new AddAdventureCommand(adventure));
            return Created(string.Empty, response.Data);
        }
    }
}