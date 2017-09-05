using System;

using Serilog.Events;

using Stove.Log;

namespace Stove.Serilog
{
	public class StoveSerilogLogger : ILogger
	{
		private readonly global::Serilog.ILogger _logger;

		public StoveSerilogLogger(global::Serilog.ILogger logger)
		{
			_logger = logger;
		}

		public string Name { get; }

		public bool IsTraceEnabled => _logger.IsEnabled(LogEventLevel.Verbose);

		public bool IsDebugEnabled => _logger.IsEnabled(LogEventLevel.Debug);

		public bool IsInfoEnabled => _logger.IsEnabled(LogEventLevel.Information);

		public bool IsWarnEnabled => _logger.IsEnabled(LogEventLevel.Warning);

		public bool IsErrorEnabled => _logger.IsEnabled(LogEventLevel.Error);

		public bool IsFatalEnabled => _logger.IsEnabled(LogEventLevel.Fatal);

		public event EventHandler<EventArgs> LoggerReconfigured;

		public void Trace<T>(T value)
		{
			_logger.Verbose(value.ToString());
		}

		public void Trace<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Verbose(value.ToString());
		}

		public void Trace(string message, Exception exception)
		{
			_logger.Verbose(exception, message);
		}

		public void Trace(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Verbose(message.ToString(formatProvider), args);
		}

		public void Trace(string message)
		{
			_logger.Verbose(message);
		}

		public void Trace(string message, params object[] args)
		{
			_logger.Verbose(message, args);
		}

		public void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Verbose(message.ToString(formatProvider), argument);
		}

		public void Trace<TArgument>(string message, TArgument argument)
		{
			_logger.Verbose(message, argument);
		}

		public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Verbose(message.ToString(formatProvider), argument1, argument2);
		}

		public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Verbose(message, argument1, argument2);
		}

		public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Verbose(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Verbose(message, argument1, argument2, argument3);
		}

		public void Debug<T>(T value)
		{
			_logger.Debug(value.ToString());
		}

		public void Debug<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Debug(value.ToString());
		}

		public void Debug(string message, Exception exception)
		{
			_logger.Debug(exception, message);
		}

		public void Debug(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Debug(message.ToString(formatProvider), args);
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
			_logger.Debug(message.ToString(formatProvider), argument);
		}

		public void Debug<TArgument>(string message, TArgument argument)
		{
			_logger.Debug(message, argument);
		}

		public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Debug(message.ToString(formatProvider), argument1, argument2);
		}

		public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Debug(message, argument1, argument2);
		}

		public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Debug(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Debug(message, argument1, argument2, argument3);
		}

		public void Info<T>(T value)
		{
			_logger.Information(value.ToString());
		}

		public void Info<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Information(value.ToString());
		}

		public void Info(string message, Exception exception)
		{
			_logger.Information(exception, message);
		}

		public void Info(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Information(message.ToString(formatProvider), args);
		}

		public void Info(string message)
		{
			_logger.Information(message);
		}

		public void Info(string message, params object[] args)
		{
			_logger.Information(message, args);
		}

		public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Information(message.ToString(formatProvider), argument);
		}

		public void Info<TArgument>(string message, TArgument argument)
		{
			_logger.Information(message, argument);
		}

		public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Information(message.ToString(formatProvider), argument1, argument2);
		}

		public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Information(message, argument1, argument2);
		}

		public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Information(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Information(message, argument1, argument2, argument3);
		}

		public void Warn<T>(T value)
		{
			_logger.Warning(value.ToString());
		}

		public void Warn<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Warning(value.ToString());
		}

		public void Warn(string message, Exception exception)
		{
			_logger.Warning(exception, message);
		}

		public void Warn(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Warning(message.ToString(formatProvider), args);
		}

		public void Warn(string message)
		{
			_logger.Warning(message);
		}

		public void Warn(string message, params object[] args)
		{
			_logger.Warning(message, args);
		}

		public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
		{
			_logger.Warning(message.ToString(formatProvider), argument);
		}

		public void Warn<TArgument>(string message, TArgument argument)
		{
			_logger.Warning(message, argument);
		}

		public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Warning(message.ToString(formatProvider), argument1, argument2);
		}

		public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Warning(message, argument1, argument2);
		}

		public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Warning(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Warning(message, argument1, argument2, argument3);
		}

		public void Error<T>(T value)
		{
			_logger.Error(value.ToString());
		}

		public void Error<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Error(value.ToString());
		}

		public void Error(string message, Exception exception)
		{
			_logger.Error(exception, message);
		}

		public void Error(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Error(message.ToString(formatProvider), args);
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
			_logger.Error(message.ToString(formatProvider), argument);
		}

		public void Error<TArgument>(string message, TArgument argument)
		{
			_logger.Error(message, argument);
		}

		public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Error(message.ToString(formatProvider), argument1, argument2);
		}

		public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Error(message, argument1, argument2);
		}

		public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Error(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Error(message, argument1, argument2, argument3);
		}

		public void Fatal<T>(T value)
		{
			_logger.Fatal(value.ToString());
		}

		public void Fatal<T>(IFormatProvider formatProvider, T value)
		{
			_logger.Fatal(value.ToString());
		}

		public void Fatal(string message, Exception exception)
		{
			_logger.Fatal(exception, message);
		}

		public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
		{
			_logger.Fatal(message.ToString(formatProvider), args);
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
			_logger.Fatal(message.ToString(formatProvider), argument);
		}

		public void Fatal<TArgument>(string message, TArgument argument)
		{
			_logger.Fatal(message, argument);
		}

		public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Fatal(message.ToString(formatProvider), argument1, argument2);
		}

		public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
		{
			_logger.Fatal(message, argument1, argument2);
		}

		public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Fatal(message.ToString(formatProvider), argument1, argument2, argument3);
		}

		public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
		{
			_logger.Fatal(message, argument1, argument2, argument3);
		}
	}
}
