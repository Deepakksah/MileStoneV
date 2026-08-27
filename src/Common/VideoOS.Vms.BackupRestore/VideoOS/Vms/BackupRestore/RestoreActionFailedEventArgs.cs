using System;
using System.Collections.Generic;

namespace VideoOS.Vms.BackupRestore;

public class RestoreActionFailedEventArgs : EventArgs
{
	public RestoreConfigActionStatus ActionStatus { get; private set; }

	public RestoreConfigError ErrorCode { get; private set; }

	public string EnglishErrorDescription { get; private set; }

	public IList<string> ErrorParameters { get; private set; }

	public Exception ActionException { get; private set; }

	public RestoreActionFailedEventArgs(RestoreConfigActionStatus status, RestoreConfigError errorCode, string englishErrorDescription, IList<string> errorParameters, Exception actionException)
	{
		ActionStatus = status;
		ErrorCode = errorCode;
		EnglishErrorDescription = englishErrorDescription;
		ErrorParameters = errorParameters;
		ActionException = actionException;
	}
}
