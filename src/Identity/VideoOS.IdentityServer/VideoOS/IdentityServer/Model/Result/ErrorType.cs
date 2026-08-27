namespace VideoOS.IdentityServer.Model.Result;

public enum ErrorType
{
	Failure,
	Validation,
	Conflict,
	NotFound,
	Forbidden,
	LegacyModelStateValidation
}
