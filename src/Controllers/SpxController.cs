using Microsoft.AspNetCore.Mvc;
using SPX_WEBAPI.Domain.Dto;
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int offset, int limit)
        {
            var spxData = await _repository.Get(offset, limit);

            return Ok(spxData);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SpxDto spxDto)
        {
            var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
            var insertedRecord = await _repository.Insert(newSpxRecord);

            return Created($"https://localhost:5000/Spx/{newSpxRecord.Id}", newSpxRecord);
        }
    }
}