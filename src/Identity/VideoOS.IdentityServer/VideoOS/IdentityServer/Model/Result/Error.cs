using System;

namespace VideoOS.IdentityServer.Model.Result;

public readonly record struct Error
{
	public string Code { get; }

	public string Description { get; }

	public ErrorType Type { get; }

	private Error(string code, string description, ErrorType type)
	{
		Code = code ?? throw new ArgumentNullException("code");
		Description = description ?? throw new ArgumentNullException("description");
		Type = type;
	}

	public static Error Failure(string description = "An unspecified error occurred.")
	{
		return new Error("Failure", description, ErrorType.Failure);
	}

	public static Error FailureWithCode(string code, string description = "An unspecified error occurred.")
	{
		return new Error(code, description, ErrorType.Failure);
	}

	public static Error Validation(string code = "Validation", string description = "A validation error has occurred.")
	{
		return new Error(code, description, ErrorType.Validation);
	}

	public static Error Conflict(string code = "Conflict", string description = "A conflict error has occurred.")
	{
		return new Error(code, description, ErrorType.Conflict);
	}

	public static Error Forbidden(string code = "Forbidden", string description = "A forbidden error has occurred.")
	{
		return new Error(code, description, ErrorType.Forbidden);
	}

	public static Error NotFound(string code = "NotFound", string description = "A not found error has occurred.")
	{
		return new Error(code, description, ErrorType.NotFound);
	}

	public static Error LegacyModelStateValidation(string code = "NotFound", string description = "A not found error has occurred.")
	{
		return new Error(code, description, ErrorType.LegacyModelStateValidation);
	}
}
