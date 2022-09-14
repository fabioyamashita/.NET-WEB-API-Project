using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SPX_WEBAPI.Domain.Dto;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Repository;
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
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public class SpxController : ControllerBase
    {
        private readonly IBaseRepository<Spx> _repository;

        public SpxController(IBaseRepository<Spx> repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute, Required] int id)
        {
            var spxRecord = await _repository.GetById(db => db.Id == id);

            if (spxRecord == null)
            {
                return NotFound("Id not found");
            }

            return Ok(spxRecord);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Spx>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRecordsWithPagination([FromQuery, Required] int offset, [FromQuery, Required] int limit)
        {
            var spxData = await _repository.Get(offset, limit);

            return Ok(spxData);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody] SpxDto spxDto)
        {
            var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
            await _repository.Insert(newSpxRecord);

            return Created($"/{GetControllerName()}/{newSpxRecord.Id}", newSpxRecord);
        }



        [HttpPost("search")]
        [ProducesResponseType(typeof(List<Spx>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchRecordsFromDateInterval(
            [FromQuery, Required] int offset, [FromQuery, Required] int limit, [FromBody] SpxDateInterval spxDateInterval)
        {
            var databaseSpxRecords = await _repository
                .Get(p => p.Date >= spxDateInterval.StartDate && p.Date <= spxDateInterval.EndDate,
                offset, limit);

            if (databaseSpxRecords == null)
            {
                return NotFound("Records not found.");
            }

            return Ok(databaseSpxRecords);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrCreateRecord([FromRoute, Required] int id, [FromBody] SpxDto spxDto)
        {
            var databaseSpxRecord = await _repository.GetById(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
                await _repository.Insert(newSpxRecord);

                return Created($"/{GetControllerName()}/{newSpxRecord.Id}", newSpxRecord);
            }

            databaseSpxRecord.EditInfo(spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);

            await _repository.Update(databaseSpxRecord);

            return Ok(databaseSpxRecord);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExistingRecord([FromRoute,Required] int id)
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
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartial([FromRoute, Required] int id, [FromBody] JsonPatchDocument spxDto)
        {
            var databaseSpxRecord = await _repository.GetById(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                return NoContent();
            }

            await _repository.UpdatePatch(databaseSpxRecord, spxDto);

            return Ok(databaseSpxRecord);
        }

        private string GetControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }
    }

}