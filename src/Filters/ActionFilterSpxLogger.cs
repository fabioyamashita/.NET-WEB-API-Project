using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
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
        private readonly CustomSpxLogs<ActionFilterSpxLogger> _customSpxLogs;
        private readonly ILogger<ActionFilterSpxLogger> _logger;
        private readonly IBaseRepository<Spx> _repository;
        private readonly ILogRepository _logRepository;
        private readonly List<int> _successStatusCodes;

        private Spx _spxPreviousState { get; set; }

        public ActionFilterSpxLogger(CustomSpxLogs<ActionFilterSpxLogger> customSpxLogs,
            ILogger<ActionFilterSpxLogger> logger,
            IBaseRepository<Spx> repository, ILogRepository logRepository)
        {
            _customSpxLogs = customSpxLogs;
            _logger = logger;
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

                    if (context.ContainsRequestMethods(HttpMethod.Put) &&
                        context.HttpContext.Response.StatusCode == StatusCodes.Status201Created)
                    {
                        id = _repository.GetLastIdAsync().Result;
                    }
                    else
                    {
                        id = int.Parse(context.HttpContext.Request.Path.ToString().Split("/").Last());
                    }

                    var spxCurrentState = _repository.GetByIdAsync(db => db.Id == id).Result;

                    _customSpxLogs.SaveLog(_logRepository, _logger, _spxPreviousState, spxCurrentState);
                }

                else if (context.ContainsRequestMethods(HttpMethod.Delete))
                {
                    _customSpxLogs.SaveLog(_logRepository, _logger, _spxPreviousState);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (String.Equals(context.ActionDescriptor.RouteValues["controller"], "spx", StringComparison.InvariantCultureIgnoreCase) &&
                context.ContainsRequestMethods(HttpMethod.Put, HttpMethod.Patch, HttpMethod.Delete))
            {
                var id = int.Parse(context.ActionArguments["id"].ToString());

                var _spxPreviousStateCopy = _repository.GetByIdAsync(db => db.Id == id).Result;

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
