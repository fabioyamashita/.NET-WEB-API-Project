using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using System;
using System.Text.Json;

namespace SPX_WEBAPI.Loggers
{
    public static class CustomSpxLogs
    {
        // Update (Put/Patch)
        public static void SaveLog(ILogRepository _logRepository, Spx spxPreviousState, Spx spxCurrentState)
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


            Console.WriteLine(message);

            _logRepository.Insert(message);
        }

        // Delete
        public static void SaveLog(ILogRepository _logRepository, Spx spxPreviousState)
        {
            var message = $"{DateTime.Now.ToString("G")} - SPX Historical Data #{spxPreviousState.Id}" +
                             $" ({spxPreviousState.Date?.ToString("d")}) - Deleted";

            Console.WriteLine(message);

            _logRepository.Insert(message);
        }
    }
}
