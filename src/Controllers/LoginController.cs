using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SPX_WEBAPI.AuthorizationAndAuthentication;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public LoginController(IUsersRepository repository, IConfiguration configuration, TokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate([FromBody] Authenticate authInfo)
        {
            var userAdmin = new Users("admin", authInfo.Login, authInfo.Password, "admin");

            if (authInfo.Login.Equals(_configuration["AdminAuthentication:login"]) &&
                authInfo.Password.Equals(_configuration["AdminAuthentication:password"]))
            {
                var token = _tokenService.GenerateToken(userAdmin);

                return Ok(token);
            }

            return Unauthorized("Incorrect login or password.");
        }

    }
}
