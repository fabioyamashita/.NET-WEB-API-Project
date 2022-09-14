using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var spxRecord = await _repository.GetById(db => db.Id == id);

            if (spxRecord == null)
            {
                return NotFound("Id not found");
            }

            return Ok(spxRecord);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecordsWithPagination([FromQuery] int offset, int limit)
        {
            var spxData = await _repository.Get(offset, limit);

            return Ok(spxData);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNew([FromBody] SpxDto spxDto)
        {
            var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
            await _repository.Insert(newSpxRecord);

            return Created($"https://localhost:5000/Spx/{newSpxRecord.Id}", newSpxRecord);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchRecordsFromDateInterval(
            [FromQuery] int offset, [FromQuery] int limit, [FromBody] SpxDateInterval spxDateInterval)
        {
            var databaseSpxRecords = await _repository
                .Get(p => p.Date >= spxDateInterval.StartDate && p.Date <= spxDateInterval.EndDate, 
                offset, limit);

            if (databaseSpxRecords == null)
            {
                return NotFound();
            }

            return Ok(databaseSpxRecords);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrCreateRecord([FromRoute] int id, [FromBody] SpxDto spxDto)
        {
            var databaseSpxRecord = await _repository.GetById(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
                await _repository.Insert(newSpxRecord);

                return Created($"https://localhost:5000/Spx/{newSpxRecord.Id}", newSpxRecord);
            }

            databaseSpxRecord.EditInfo(spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);

            await _repository.Update(databaseSpxRecord);

            return Ok(databaseSpxRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExistingRecord([FromRoute] int id)
        {
            var databaseSpxRecord = await _repository.GetById(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                return NoContent();
            }

            await _repository.Delete(databaseSpxRecord);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePartial([FromRoute] int id, [FromBody] JsonPatchDocument spxDto)
        {
            var databaseSpxRecord = await _repository.GetById(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                return NotFound("Id not found");
            }

            await _repository.UpdatePatch(databaseSpxRecord, spxDto);

            return Ok(databaseSpxRecord);
        }

    }
}