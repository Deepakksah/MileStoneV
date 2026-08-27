namespace VideoOS.DatabaseUtility.MediaStorage;

public struct VerificationResult(ResultItem recorded, ResultItem exported)
{
	public ResultItem RecordedSignatures = recorded;

	public ResultItem ExportedSignatures = exported;
}
