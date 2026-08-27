namespace VideoOS.Vms.BackupRestore;

public abstract class ValidationResult
{
	public bool Success { get; set; }

	public string EnglishErrorDescription { get; set; }

	public string[] ErrorParameters { get; private set; }

	protected ValidationResult(bool success, string englishErrorDescription, string[] errorParameters)
	{
		Success = success;
		EnglishErrorDescription = englishErrorDescription;
		ErrorParameters = errorParameters;
	}
}
