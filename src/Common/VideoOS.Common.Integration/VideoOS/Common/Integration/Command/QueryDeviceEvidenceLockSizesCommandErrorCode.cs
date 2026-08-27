namespace VideoOS.Common.Integration.Command;

public enum QueryDeviceEvidenceLockSizesCommandErrorCode
{
	EvidenceLockNotFound,
	DatabaseCommunicationError,
	SetEvidenceLocksNotCalled,
	MediaDbRepairInProgress
}
