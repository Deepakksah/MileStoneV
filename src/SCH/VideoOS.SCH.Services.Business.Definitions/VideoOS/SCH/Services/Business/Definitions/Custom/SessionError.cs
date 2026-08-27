namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public enum SessionError
{
	SessionNotFound = -201000,
	EndSessionFailed = -201001,
	SessionRemoveFailed = -201002,
	InvalidSessionDataClientName = -201003,
	InvalidSessionDataClientNameTooLong = -201004,
	InvalidSessionDataClientUserNameTooLong = -201005
}
