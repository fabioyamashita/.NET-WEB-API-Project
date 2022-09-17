using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPX_WEBAPI.Domain.Dto;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;

        public UsersController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Spx>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsersWithPagination([FromQuery, Required] int offset, int limit)
        {
            var users = await _repository.GetAsync(offset, limit);
            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> InsertNewUser([FromBody] UsersDto usersDto)
        {
            var userFound = await _repository.GetAsync(usersDto.Username);

            if (userFound != null)
            {
                return Conflict("User already exists");
            }

            var user = new Users(usersDto.Name, usersDto.Username, usersDto.Password, usersDto.Role);
            await _repository.InsertAsync(user);
            return Created("", user);
        }
    }
}
