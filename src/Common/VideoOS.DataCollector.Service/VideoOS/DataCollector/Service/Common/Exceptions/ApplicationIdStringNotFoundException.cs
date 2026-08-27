using System;

namespace VideoOS.DataCollector.Service.Common.Exceptions;

public class ApplicationIdStringNotFoundException : InvalidOperationException
{
	public ApplicationIdStringNotFoundException()
		: base("Application identification string was not found in registry.")
	{
	}

	public ApplicationIdStringNotFoundException(string message)
		: base(message)
	{
	}

	public ApplicationIdStringNotFoundException(string message, Exception exception)
		: base(message, exception)
	{
	}
}
