using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using SPX_WEBAPI.Loggers;
using SPX_WEBAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPX_WEBAPI.Filters
{
    public class ActionFilterSpxLogger : IResultFilter, IActionFilter
    {
        private readonly IBaseRepository<Spx> _repository;
        private readonly ILogRepository _logRepository;
        private readonly List<int> _successStatusCodes;

        private Spx _spxPreviousState { get; set; }

        public ActionFilterSpxLogger(IBaseRepository<Spx> repository, ILogRepository logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
            _successStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created };
            _spxPreviousState = null;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/Spx/", StringComparison.InvariantCultureIgnoreCase) &&
                _successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
            {
                if (context.ContainsRequestMethods(HttpMethod.Put, HttpMethod.Patch))
                {
                    var id = 0;

                    if (context.ContainsRequestMethods(HttpMethod.Put))
                    {
                        id = _repository.Get(_repository.CountTotalRecords().Result, 1).Result.First().Id;
                    }
                    else if (context.ContainsRequestMethods(HttpMethod.Patch))
                    {
                        id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                    }

                    var spxCurrentState = _repository.GetById(db => db.Id == id).Result;

                    CustomSpxLogs.SaveLog(_logRepository, _spxPreviousState, spxCurrentState);
                }

                else if (context.ContainsRequestMethods(HttpMethod.Delete))
                {
                    CustomSpxLogs.SaveLog(_logRepository, _spxPreviousState);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (String.Equals(context.ActionDescriptor.RouteValues["controller"], "spx", StringComparison.InvariantCultureIgnoreCase) &&
                context.ContainsRequestMethods(HttpMethod.Put, HttpMethod.Patch, HttpMethod.Delete))
            {
                var id = int.Parse(context.ActionArguments["id"].ToString());

                var _spxPreviousStateCopy = _repository.GetById(db => db.Id == id).Result;

                if (_spxPreviousStateCopy != null)
                {
                    _spxPreviousState = (Spx)_spxPreviousStateCopy.Clone();
                }
            }
        }

        #region "Not implemented"
        public void OnResultExecuting(ResultExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        #endregion

    }
}
