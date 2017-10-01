using System;

using NLog;

using ILogger = Stove.Log.ILogger;

namespace Stove.NLog
{
	/// <summary>
	///     Implementation of ILogger, deriving from NLog.Logger
	/// </summary>
	public class StoveNLogLogger : ILogger
	{
		private readonly Logger _logger;

		/// <summary>
		///     Create an adapter class using a Logger instance
		/// </summary>
		/// <param name="logger"></param>
		public StoveNLogLogger(Logger logger)
		{
			_logger = logger;
		}

		/// <summary>
		///     Occurs when logger configuration changes.
		/// </summary>
		public event EventHandler<EventArgs> LoggerReconfigured
		{
			add { _logger.LoggerReconfigured += value; }
			remove { _logger.LoggerReconfigured -= value; }
		}

		public string Name => _logger.Name;

		public bool IsTraceEnabled => _logger.IsTraceEnabled;

		public bool IsDebugEnabled => _logger.IsDebugEnabled;

		public bool IsInfoEnabled => _logger.IsInfoEnabled;

		public bool IsWarnEnabled => _logger.IsWarnEnabled;

		public bool IsErrorEnabled => _logger.IsErrorEnabled;

		public bool IsFatalEnabled => _logger.IsErrorEnabled;

		public void Trace<T>(T value)
		{
			_logger.Trace(value);
		}

		public void Trace<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Trace(formatProvider, value);
		}

		public void Trace(string message, Exception exception)
		{
			_logger.Trace(exception, message);
		}

		public void Trace(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Trace(formatProvider, message, args);
		}

		public void Trace(string message)
		{
			_logger.Trace(message);
		}

		public void Trace(string message, params object[] args)
		{
			_logger.Trace(message, args);
		}

		public void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Trace(formatProvider, message, argument);
		}

		public void Trace<TArgument>(string message, TArgument argument)
		{
			_logger.Trace(message, argument);
		}

		public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Trace(formatProvider, message, argument1, argument2);
		}

		public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Trace(message, argument1, argument2);
		}

		public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Trace(formatProvider, message, argument1, argument2, argument3);
		}

		public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Trace(message, argument1, argument2, argument3);
		}

		public void Debug<T>(T value)
		{
			_logger.Debug(value);
		}

		public void Debug<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Debug(formatProvider, value);
		}

		public void Debug(string message, Exception exception)
		{
			_logger.Debug(exception, message);
		}

		public void Debug(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Debug(formatProvider, message, args);
		}

		public void Debug(string message)
		{
			_logger.Debug(message);
		}

		public void Debug(string message, params object[] args)
		{
			_logger.Debug(message, args);
		}

		public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Debug(formatProvider, message, argument);
		}

		public void Debug<TArgument>(string message, TArgument argument)
		{
			_logger.Debug(message, argument);
		}

		public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Debug(formatProvider, message, argument1, argument2);
		}

		public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Debug(message, argument1, argument2);
		}

		public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Debug(formatProvider, message, argument1, argument2, argument3);
		}

		public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Debug(message, argument1, argument2, argument3);
		}

		public void Info<T>(T value)
		{
			_logger.Info(value);
		}

		public void Info<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Info(formatProvider, value);
		}

		public void Info(string message, Exception exception)
		{
			_logger.Info(exception, message);
		}

		public void Info(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Info(formatProvider, message, args);
		}

		public void Info(string message)
		{
			_logger.Info(message);
		}

		public void Info(string message, params object[] args)
		{
			_logger.Info(message, args);
		}

		public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Info(formatProvider, message, argument);
		}

		public void Info<TArgument>(string message, TArgument argument)
		{
			_logger.Info(message, argument);
		}

		public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Info(formatProvider, message, argument1, argument2);
		}

		public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Info(message, argument1, argument2);
		}

		public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Info(formatProvider, message, argument1, argument2, argument3);
		}

		public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Info(message, argument1, argument2, argument3);
		}

		public void Warn<T>(T value)
		{
			_logger.Warn(value);
		}

		public void Warn<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Warn(formatProvider, value);
		}

		public void Warn(string message, Exception exception)
		{
			_logger.Warn(exception, message);
		}

		public void Warn(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Warn(formatProvider, message, args);
		}

		public void Warn(string message)
		{
			_logger.Warn(message);
		}

		public void Warn(string message, params object[] args)
		{
			_logger.Warn(message, args);
		}

		public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Warn(formatProvider, message, argument);
		}

		public void Warn<TArgument>(string message, TArgument argument)
		{
			_logger.Warn(message, argument);
		}

		public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Warn(formatProvider, message, argument1, argument2);
		}

		public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Warn(message, argument1, argument2);
		}

		public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Warn(formatProvider, message, argument1, argument2, argument3);
		}

		public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Warn(message, argument1, argument2, argument3);
		}

		public void Error<T>(T value)
		{
			_logger.Error(value);
		}

		public void Error<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Error(formatProvider, value);
		}

		public void Error(string message, Exception exception)
		{
			_logger.Error(exception, message);
		}

		public void Error(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Error(formatProvider, message, args);
		}

		public void Error(string message)
		{
			_logger.Error(message);
		}

		public void Error(string message, params object[] args)
		{
			_logger.Error(message, args);
		}

		public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Error(formatProvider, message, argument);
		}

		public void Error<TArgument>(string message, TArgument argument)
		{
			_logger.Error(message, argument);
		}

		public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Error(formatProvider, message, argument1, argument2);
		}

		public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Error(message, argument1, argument2);
		}

		public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Error(formatProvider, message, argument1, argument2, argument3);
		}

		public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Error(message, argument1, argument2, argument3);
		}

		public void Fatal<T>(T value)
		{
			_logger.Fatal(value);
		}

		public void Fatal<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Fatal(formatProvider, value);
		}

		public void Fatal(string message, Exception exception)
		{
			_logger.Fatal(exception, message);
		}

		public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Fatal(formatProvider, message, args);
		}

		public void Fatal(string message)
		{
			_logger.Fatal(message);
		}

		public void Fatal(string message, params object[] args)
		{
			_logger.Fatal(message, args);
		}

		public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Fatal(formatProvider, message, argument);
		}

		public void Fatal<TArgument>(string message, TArgument argument)
		{
			_logger.Fatal(message, argument);
		}

		public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Fatal(formatProvider, message, argument1, argument2);
		}

		public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Fatal(message, argument1, argument2);
		}

		public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Fatal(formatProvider, message, argument1, argument2, argument3);
		}

		public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Fatal(message, argument1, argument2, argument3);
		}

		public void Log(LogEventInfo logEvent)
		{
			_logger.Log(logEvent);
		}

		public void Log(Type wrapperType, LogEventInfo logEvent)
		{
			_logger.Log(wrapperType, logEvent);
		}
	}
}
