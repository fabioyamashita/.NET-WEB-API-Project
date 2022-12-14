using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
    public class SpxController : ControllerBase
    {
        private readonly IBaseRepository<Spx> _repository;

        public SpxController(IBaseRepository<Spx> repository)
        {
            _repository = repository;
        }

        #region "GET - GetById"
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute, Required] int id)
        {
            var spxRecord = await _repository.GetByIdAsync(db => db.Id == id);

            if (spxRecord == null)
            {
                return NotFound("Id not found");
            }

            return Ok(spxRecord);
        }
        #endregion

        #region "GET - GetAllRecordsWithPagination"
        [HttpGet]
        [ProducesResponseType(typeof(List<Spx>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllRecordsWithPagination([FromQuery, Required] int offset, [FromQuery, Required] int limit)
        {
            var spxData = await _repository.GetAsync(offset, limit);

            return Ok(spxData);
        }
        #endregion

        #region "POST - CreateNew"
        [HttpPost]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody] SpxDto spxDto)
        {
            var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
            await _repository.InsertAsync(newSpxRecord);

            return Created($"/{GetControllerName()}/{newSpxRecord.Id}", newSpxRecord);
        }
        #endregion

        #region "POST - SearchRecordsFromDateInterval"
        [HttpPost("search")]
        [ProducesResponseType(typeof(List<Spx>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchRecordsFromDateInterval(
            [FromQuery, Required] int offset, [FromQuery, Required] int limit, [FromBody] SpxDateInterval spxDateInterval)
        {
            var databaseSpxRecords = await _repository
                .GetAsync(p => p.Date >= spxDateInterval.StartDate && p.Date <= spxDateInterval.EndDate,
                offset, limit);

            if (databaseSpxRecords == null)
            {
                return NotFound("Records not found.");
            }

            return Ok(databaseSpxRecords);
        }
        #endregion

        #region "PUT - UpdateOrCreateRecord"
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrCreateRecord([FromRoute, Required] int id, [FromBody] SpxDto spxDto)
        {
            var databaseSpxRecord = await _repository.GetByIdAsync(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                var newSpxRecord = new Spx(id: 0, spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);
                await _repository.InsertAsync(newSpxRecord);

                return Created($"/{GetControllerName()}/{newSpxRecord.Id}", newSpxRecord);
            }

            databaseSpxRecord.EditInfo(spxDto.Date, spxDto.Close, spxDto.Open, spxDto.High, spxDto.Low);

            await _repository.UpdateAsync(databaseSpxRecord);

            return Ok(databaseSpxRecord);
        }
        #endregion

        #region "DELETE - DeleteExistingRecord"
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExistingRecord([FromRoute,Required] int id)
        {
            var databaseSpxRecord = await _repository.GetByIdAsync(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                return NotFound("Id not found");
            }

            await _repository.DeleteAsync(databaseSpxRecord);

            return Ok();
        }
        #endregion

        #region "PATCH - UpdatePartial"
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Spx), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartial([FromRoute, Required] int id, [FromBody] JsonPatchDocument spxDto)
        {
            var databaseSpxRecord = await _repository.GetByIdAsync(db => db.Id == id);

            if (databaseSpxRecord == null)
            {
                return NotFound("Id not found");
            }

            await _repository.UpdatePatchAsync(databaseSpxRecord, spxDto);

            return Ok(databaseSpxRecord);
        }
        #endregion

        private string GetControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"].ToString();
        }
    }

}