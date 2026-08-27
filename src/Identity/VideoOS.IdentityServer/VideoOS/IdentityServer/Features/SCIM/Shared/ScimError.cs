using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimError
{
	public record ScimErrorDefinition(string Type, int StatusCode);

	public static class ScimTypes
	{
		public const string Uniqueness = "uniqueness";

		public const string InvalidValue = "invalidValue";

		public const string NotFound = "notFound";

		public const string InsufficientPermissions = "insufficientPermissions";

		public const string InvalidFilter = "invalidFilter";

		public const string InvalidPath = "invalidPath";

		public const string NoTarget = "noTarget";
	}

	public static readonly ScimErrorDefinition ErrorUniqueness = new ScimErrorDefinition("uniqueness", 409);

	public static readonly ScimErrorDefinition ErrorInvalidValue = new ScimErrorDefinition("invalidValue", 400);

	public static readonly ScimErrorDefinition ErrorInvalidPath = new ScimErrorDefinition("invalidPath", 400);

	public static readonly ScimErrorDefinition ErrorNoTarget = new ScimErrorDefinition("noTarget", 400);

	public static readonly ScimErrorDefinition ErrorNotFound = new ScimErrorDefinition("notFound", 404);

	public static readonly ScimErrorDefinition ErrorForbidden = new ScimErrorDefinition("insufficientPermissions", 403);

	public static readonly ScimErrorDefinition ErrorInvalidFilter = new ScimErrorDefinition("invalidFilter", 400);

	public static readonly ScimErrorDefinition ErrorUnknown = new ScimErrorDefinition(null, 400);

	public static ScimErrorDefinition GetError(Error error)
	{
		switch (error.Type)
		{
		case ErrorType.Conflict:
			return ErrorUniqueness;
		case ErrorType.NotFound:
			return ErrorNotFound;
		case ErrorType.Validation:
		{
			string code = error.Code;
			if (code != null && code.Contains("invalidFilter"))
			{
				return ErrorInvalidFilter;
			}
			string code2 = error.Code;
			if (code2 != null && code2.Contains("invalidPath"))
			{
				return ErrorInvalidPath;
			}
			string code3 = error.Code;
			if (code3 != null && code3.Contains("noTarget"))
			{
				return ErrorNoTarget;
			}
			return ErrorInvalidValue;
		}
		case ErrorType.Forbidden:
			return ErrorForbidden;
		default:
			return ErrorUnknown;
		}
	}
}
