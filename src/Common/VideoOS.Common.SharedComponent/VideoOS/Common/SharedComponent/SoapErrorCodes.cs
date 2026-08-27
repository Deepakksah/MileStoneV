namespace VideoOS.Common.SharedComponent;

public static class SoapErrorCodes
{
	public enum SoapErrorCode
	{
		CorporateClientRequestRejectDueToTimeProfile = 50000,
		CorporateClientRequestReject = 51000,
		CorpoarteClientRequestRejectedDueToDualAuthorizationRequired = 51100,
		CorporateServerServiceNotStarted = 52000,
		CorporateAuthorizationRejectedDueToSelfAuthorization = 54000,
		CorporateAuthorizationRejectedDueToNotMemberOfAuthorizeRole = 54100,
		CorporateAuthorizationRejectedDueAuthorizationNotRequired = 54200,
		CorporatePresetDuplicateName = 55001,
		CorporatePresetUsedInPatrolingProfile = 55002,
		CorporatePresetNotSupportedOnDevice = 55003,
		CorporatePresetNameAsKey = 55004,
		CorporatePresetNotUpdateable = 55005
	}
}
