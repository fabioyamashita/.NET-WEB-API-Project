using Microsoft.AspNetCore.Mvc;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Repository;
using System.Threading.Tasks;

namespace SPX_WEBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpxController : ControllerBase
    {
        private readonly IBaseRepository<Spx> _repository;

        public SpxController(IBaseRepository<Spx> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var spxRecord = await _repository.GetByKey(id);

            if (spxRecord == null)
            {
                return NotFound("Id not found");
            }

            return Ok(spxRecord);
        }
    }
}