namespace VideoOS.Platform;

public enum MIPSoapErrorCode
{
	None = 0,
	EnterpriseServerUnknown = 10000,
	EnterpriseClientSoap = 10002,
	EnterpriseClientMethod = 10003,
	EnterpriseClientRequestRejectedDueToDualAuthorizationRequired = 10100,
	EnterpriseClientAuthorizationRejectedDueToSelfAuthorization = 14000,
	EnterpriseClientAuthorizationRejectedDueToNotMemberOfAuthorizeRole = 14100,
	EnterpriseClientToken = 20001,
	EnterpriseServerTokenMax = 20002,
	EnterpriseClientPermission = 20003,
	EnterpriseClientParameterInvalid = 30001,
	EnterpriseClientDevice = 30002,
	EnterpriseServerOperation = 40000,
	EnterprisePresetLimit = 40001,
	EnterprisePresetDuplicateName = 40002,
	CorporateClientRequestRejectDueToTimeProfile = 50000,
	CorporateClientRequestReject = 51000,
	CorporateClientRequestRejectedDueToDualAuthorizationRequired = 51100,
	CorporateServerServiceNotStarted = 52000,
	CorporateAuthorizationRejectedDueToSelfAuthorization = 54000,
	CorporateAuthorizationRejectedDueToNotMemberOfAuthorizeRole = 54100,
	CorporateAuthorizationRejectedDueAuthorizationNotRequired = 54200,
	CorporatePresetDuplicateName = 55001,
	CorporatePresetUsedInPatrollingProfile = 55002
}
