using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACPropertyValidationResult
{
	public static readonly ACPropertyValidationResult Success = new ACPropertyValidationResult();

	[DataMember]
	public bool ValidatedOk { get; private set; }

	[DataMember]
	public string FirstErrorPropertyKey { get; private set; }

	[DataMember]
	public string ErrorText { get; private set; }

	public ACPropertyValidationResult()
	{
		ValidatedOk = true;
		FirstErrorPropertyKey = string.Empty;
		ErrorText = string.Empty;
	}

	public ACPropertyValidationResult(string firstErrorPropertyKey, string errorText)
	{
		ValidatedOk = false;
		FirstErrorPropertyKey = firstErrorPropertyKey;
		ErrorText = errorText;
	}
}
