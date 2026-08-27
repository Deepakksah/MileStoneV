namespace VideoOS.Reporting.Web.Plugin.Base;

public enum LogType
{
	Success = 0,
	Error = 1,
	Warning = 2,
	Information = 4,
	AuditSuccess = 8,
	AuditFailure = 0x10,
	Debug = 0x20
}
