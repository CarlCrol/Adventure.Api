using Adventure.Core;
using Adventure.Core.UserAdventures.Commands;
using Adventure.Core.UserAdventures.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAdventureController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserAdventureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get User Adventures
        /// </summary>
        /// <param name="username"></param>
        /// <param name="currentPage"></param>
        /// <param name="maxItems"></param>
        /// <response code="200">Returns the User Adventures</response>
        [HttpGet("{username}/{currentPage}/{maxItems}")]
        [ProducesResponseType(typeof(ServiceResponse<GetUserAdventuresResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string username, int currentPage, int maxItems)
        {
            return Ok(await _mediator.Send(new GetUserAdventuresQuery(username, currentPage, maxItems)));
        }

        /// <summary>
        /// Adds new User Adventure
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="adventureId"></param>
        /// <param name="selectedRoutes"></param>
        /// <response code="201">Returns the identifier of the new User Adventures</response>
        /// <response code="400">if new User Adventures has invalid properties</response>
        [HttpPost("{userId}/{adventureId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(int userId, int adventureId, [FromBody] List<Dto.SelectedRoute> selectedRoutes)
        {
            var response = await _mediator.Send(new AddUserAdventureCommand(userId, adventureId, selectedRoutes));
            return Created(string.Empty, response.Data);
        }
    }
}