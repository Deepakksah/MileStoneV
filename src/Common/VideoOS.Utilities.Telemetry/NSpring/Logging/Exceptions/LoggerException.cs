using System;
using System.Collections;

namespace NSpring.Logging.Exceptions;

public sealed class LoggerException : ApplicationException
{
	private const string Exception_DESCRIPTION = "Logging exception";

	private const string BATCH_Exception_DESCRIPTION = "Batch logging exception";

	private ArrayList exceptionsList = new ArrayList();

	public readonly bool IsBatchException;

	public readonly Logger Logger;

	public readonly Event Event;

	public readonly Exception Exception;

	public LoggerException[] Exceptions
	{
		get
		{
			lock (exceptionsList)
			{
				LoggerException[] array = new LoggerException[exceptionsList.Count];
				exceptionsList.CopyTo(array);
				return array;
			}
		}
	}

	public LoggerException(Logger _logger, Event _event, Exception _exception)
		: base("Logging exception", _exception)
	{
		if (_logger == null || _event == null || _exception == null)
		{
			throw new ArgumentNullException();
		}
		Logger = _logger;
		Event = _event;
		Exception = _exception;
		IsBatchException = false;
	}

	public LoggerException(LoggerException[] exceptions)
		: base("Batch logging exception")
	{
		Exception = null;
		IsBatchException = true;
		if (exceptions == null)
		{
			return;
		}
		foreach (LoggerException ex in exceptions)
		{
			if (ex != null)
			{
				exceptionsList.Add(ex);
			}
		}
	}

	public LoggerException()
		: base("Batch logging exception")
	{
		Exception = null;
		IsBatchException = true;
	}

	public static LoggerException CreateBatchException(Logger _logger, Event[] _events, Exception _exception)
	{
		if (_logger == null || _events == null || _exception == null)
		{
			throw new ArgumentNullException();
		}
		LoggerException ex = new LoggerException();
		foreach (Event obj in _events)
		{
			if (obj != null)
			{
				ex.AddException(_logger, obj, _exception);
			}
		}
		return ex;
	}

	public void AddException(Logger _logger, Event _event, Exception _exception)
	{
		if (_logger == null || _event == null || _exception == null)
		{
			throw new ArgumentNullException();
		}
		lock (exceptionsList)
		{
			exceptionsList.Add(new LoggerException(_logger, _event, _exception));
		}
	}
}
