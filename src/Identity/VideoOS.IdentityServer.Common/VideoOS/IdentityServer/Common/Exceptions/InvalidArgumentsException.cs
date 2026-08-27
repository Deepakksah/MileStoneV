using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class InvalidArgumentsException : IdpException
{
	public Collection<string> Arguments { get; } = new Collection<string>();

	public InvalidArgumentsException(ErrorResponse errors)
		: base(400, errors?.Errors, errors?.Error)
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
		if (errors.Extensions == null)
		{
			throw new ArgumentException("Errors must contain 'extensions' property.", "errors");
		}
		if (!errors.Extensions.TryGetValue("reason", out var value))
		{
			throw new ArgumentException("Errors must contain 'reason' extension property.", "errors");
		}
		if (!string.Equals(value.ToString(), "ModelValidationError", StringComparison.Ordinal))
		{
			throw new ArgumentException("Errors must have 'reason' model validation error.", "errors");
		}
		foreach (KeyValuePair<string, string[]> error in errors.Errors)
		{
			Arguments.Add(error.Key);
		}
	}
}
