using Microsoft.Extensions.Logging;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using System;
using System.Text.Json;

namespace SPX_WEBAPI.Loggers
{
    public class CustomSpxLogs<T> where T : class
    {
        // Update (Put/Patch)
        public void SaveLog(ILogRepository _logRepository, ILogger<T> logger ,Spx spxPreviousState, Spx spxCurrentState)
        {
            var message = "";

            if (spxPreviousState == null)
            {
                message = $"{DateTime.Now.ToString("G")} - SPX Historical Data #{spxCurrentState.Id} "+
                            $"({spxCurrentState.Date?.ToString("d")}) - Created " +
                            $"{JsonSerializer.Serialize(spxCurrentState)}";

            }
            else
            {
                message = $"{DateTime.Now.ToString("G")} - SPX Historical Data #{spxCurrentState.Id} " +
                            $"({spxCurrentState.Date?.ToString("d")}) " +
                            $"- Updated from:" +
                            $"{JsonSerializer.Serialize(spxPreviousState)} " +
                            $"to {JsonSerializer.Serialize(spxCurrentState)}";
            }

            logger.LogInformation(message);

            _logRepository.Insert(message);
        }

        // Delete
        public void SaveLog(ILogRepository _logRepository, ILogger logger, Spx spxPreviousState)
        {
            var message = $"{DateTime.Now.ToString("G")} - SPX Historical Data #{spxPreviousState.Id}" +
                             $" ({spxPreviousState.Date?.ToString("d")}) - Deleted";

            logger.LogInformation(message);

            _logRepository.Insert(message);
        }
    }
}
