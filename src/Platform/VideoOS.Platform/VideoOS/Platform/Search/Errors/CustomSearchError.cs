using System;

namespace VideoOS.Platform.Search.Errors;

public class CustomSearchError : SearchError
{
	public Exception Exception { get; protected set; }

	public string DisplayMessage => base.Message;

	protected CustomSearchError()
	{
	}

	public CustomSearchError(string message)
		: this(message, SearchErrorType.Error)
	{
	}

	public CustomSearchError(string message, SearchErrorType errorType)
	{
		base.Message = message;
		base.ErrorType = errorType;
	}

	public CustomSearchError(Exception exception)
		: this(exception, SearchErrorType.Error)
	{
	}

	public CustomSearchError(Exception exception, SearchErrorType errorType)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		base.Message = exception.Message;
		Exception = exception;
		base.ErrorType = errorType;
	}
}
