using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.IdentityServer.Common.Exceptions;

public abstract class IdpException : Exception
{
	public int StatusCode { get; }

	public IReadOnlyDictionary<string, string[]> Errors { get; }

	public string Error { get; }

	public override string Message
	{
		get
		{
			if (Errors != null && Errors.Any())
			{
				return FormattableString.Invariant($"{base.Message} Idp returned {StatusCode} with the errors:{Environment.NewLine}{FormatErrors()}");
			}
			if (Error != null)
			{
				return FormattableString.Invariant($"{base.Message} Idp returned {StatusCode} with the error: {Error}.");
			}
			return FormattableString.Invariant($"{base.Message} Idp returned {StatusCode}.");
		}
	}

	protected IdpException(int statusCode, IReadOnlyDictionary<string, string[]> errors, string error)
	{
		if (statusCode < 400 || statusCode > 599)
		{
			throw new ArgumentOutOfRangeException("statusCode", statusCode, "Must be in the range 400-599.");
		}
		StatusCode = statusCode;
		Errors = errors;
		Error = error;
	}

	private string FormatErrors()
	{
		return string.Join(Environment.NewLine, Errors.Select((KeyValuePair<string, string[]> kv) => kv.Key + ": " + string.Join(", ", kv.Value)));
	}
}
