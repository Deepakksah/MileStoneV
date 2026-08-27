using System;

namespace VideoOS.Vms.BackupRestore;

public abstract class ComponentResult
{
	public bool Success { get; set; }

	public string EnglishErrorDescription { get; set; }

	public string[] ErrorParameters { get; private set; }

	public Exception Exception { get; set; }

	protected ComponentResult(bool success, string englishErrorDescription, string[] errorParameters, Exception exception)
	{
		Success = success;
		EnglishErrorDescription = englishErrorDescription;
		ErrorParameters = errorParameters;
		Exception = exception;
	}
}
