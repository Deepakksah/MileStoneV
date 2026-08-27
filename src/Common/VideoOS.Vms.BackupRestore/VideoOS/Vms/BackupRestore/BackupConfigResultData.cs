using System;
using System.Collections.Generic;

namespace VideoOS.Vms.BackupRestore;

public class BackupConfigResultData
{
	public BackupConfigManager.BackupConfigResult Result { get; private set; }

	public BackupConfigManager.BackupConfigActionStatus ActionStatus { get; private set; }

	public BackupConfigManager.BackupConfigError Error { get; private set; }

	public IList<string> FileNamesToReturn { get; private set; }

	public string EnglishErrorDescription { get; set; }

	public string[] ErrorParameters { get; private set; }

	public Exception ServerException { get; set; }

	private BackupConfigResultData(BackupConfigManager.BackupConfigResult result, BackupConfigManager.BackupConfigActionStatus actionStatus, BackupConfigManager.BackupConfigError error, IList<string> fileNamesToReturn, string englishErrorDescr, string[] errorParameters, Exception serverException)
	{
		Result = result;
		ActionStatus = actionStatus;
		Error = error;
		FileNamesToReturn = fileNamesToReturn;
		EnglishErrorDescription = englishErrorDescr;
		ErrorParameters = errorParameters;
		ServerException = serverException;
	}

	public static BackupConfigResultData CreateSuccessResult(IList<string> fileNamesToReturn)
	{
		return new BackupConfigResultData(BackupConfigManager.BackupConfigResult.Success, BackupConfigManager.BackupConfigActionStatus.Undefined, BackupConfigManager.BackupConfigError.Undefined, fileNamesToReturn, null, null, null);
	}

	public static BackupConfigResultData CreateAbortedResult()
	{
		return new BackupConfigResultData(BackupConfigManager.BackupConfigResult.Aborted, BackupConfigManager.BackupConfigActionStatus.Undefined, BackupConfigManager.BackupConfigError.Undefined, null, null, null, null);
	}

	public static BackupConfigResultData CreateFailedResult(BackupConfigManager.BackupConfigError backupError)
	{
		return new BackupConfigResultData(BackupConfigManager.BackupConfigResult.Failed, BackupConfigManager.BackupConfigActionStatus.Undefined, backupError, null, null, null, null);
	}

	public static BackupConfigResultData CreateFailedResult(BackupConfigManager.BackupConfigError backupError, string englishErrorDescription, string[] errorParameters, Exception serverException)
	{
		return new BackupConfigResultData(BackupConfigManager.BackupConfigResult.Failed, BackupConfigManager.BackupConfigActionStatus.Undefined, backupError, null, englishErrorDescription, errorParameters, serverException);
	}
}
