using System;

namespace VideoOS.Vms.BackupRestore;

public class RestoreComponentResult : ComponentResult
{
	public RestoreConfigError ComponentError { get; set; }

	protected RestoreComponentResult(bool success, RestoreConfigError componentError, string englishErrorDescription, string[] errorParameters, Exception exception)
		: base(success, englishErrorDescription, errorParameters, exception)
	{
		ComponentError = componentError;
	}

	public static RestoreComponentResult CreateSuccessResult()
	{
		return new RestoreComponentResult(success: true, RestoreConfigError.Undefined, null, null, null);
	}

	public static RestoreComponentResult CreateErrorResult(RestoreConfigError componentError, string englishErrorDescription, string[] errorParameters, Exception exception)
	{
		return new RestoreComponentResult(success: false, componentError, englishErrorDescription, errorParameters, exception);
	}
}
