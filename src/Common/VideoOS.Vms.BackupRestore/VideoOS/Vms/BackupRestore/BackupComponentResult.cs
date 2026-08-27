using System;
using System.Collections.Generic;

namespace VideoOS.Vms.BackupRestore;

public class BackupComponentResult : ComponentResult
{
	public IList<string> FilesHandled { get; private set; }

	public BackupConfigManager.BackupConfigError ComponentError { get; set; }

	protected BackupComponentResult(bool success, IEnumerable<string> filesHandled, BackupConfigManager.BackupConfigError componentError, string englishErrorDescription, string[] errorParameters, Exception exception)
		: base(success, englishErrorDescription, errorParameters, exception)
	{
		if (filesHandled != null)
		{
			FilesHandled = new List<string>(filesHandled);
		}
		ComponentError = componentError;
	}

	public static BackupComponentResult CreateSuccessResult(IEnumerable<string> filesHandled)
	{
		return new BackupComponentResult(success: true, filesHandled, BackupConfigManager.BackupConfigError.Undefined, null, null, null);
	}

	public static BackupComponentResult CreateErrorResult(BackupConfigManager.BackupConfigError componentError, string englishErrorDescription, string[] errorParameters, Exception exception)
	{
		return new BackupComponentResult(success: false, null, componentError, englishErrorDescription, errorParameters, exception);
	}
}
