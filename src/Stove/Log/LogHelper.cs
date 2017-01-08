using System;

namespace Stove.Log
{
    /// <summary>
    ///     This class can be used to write logs from somewhere where it's a hard to get a reference to the
    ///     <see cref="ILogger" />.
    ///     Normally, use <see cref="ILogger" /> with property injection wherever it's possible.
    /// </summary>
    public static class LogHelper
    {
        public static void LogException(this ILogger logger, Exception ex)
        {
            LogSeverity severity = (ex as IHasLogSeverity)?.Severity ?? LogSeverity.Error;

            logger.Log(severity, ex.Message, ex);
        }
    }
}
