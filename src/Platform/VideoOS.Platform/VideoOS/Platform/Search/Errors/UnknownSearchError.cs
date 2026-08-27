using System;

namespace VideoOS.Platform.Search.Errors;

public class UnknownSearchError : SearchError
{
	public Exception Exception { get; protected set; }

	public UnknownSearchError(Exception exception)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		base.Message = exception.Message;
		Exception = exception;
	}
}
