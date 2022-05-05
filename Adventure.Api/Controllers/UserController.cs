using Adventure.Core.Administration.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Adventure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Adds new User
        /// </summary>
        /// <param name="user"></param>
        /// <response code="201">Returns the identifier of the new User</response>
        /// <response code="400">if new User has invalid properties</response>
        /// <response code="409">if new User has conflict to existing adventures</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Add([FromBody] Dto.User user)
        {
            var response = await _mediator.Send(new AddUserCommand(user));
            return Created(string.Empty, response.Data);
        }
    }
}