namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public enum AppDataError
{
	RefIntegrErrBadMsgRefInSession = -206000,
	RefIntegrErrDelMsgRefInSessFailed = -206001,
	RefIntegrErrBadSessRefInMsgWatch = -206002,
	RefIntegrErrSessToMsgRefAlreadyExist = -206003,
	NullOrEmptyKeyNotAllowed = -206004,
	UniqueKeyCheckFailed = -206005,
	UniqueKeyNotFound = -206006,
	DataRecordNotFound = -206007,
	NullOrEmptyFieldNotAllowed = -206008,
	RefIntegrErrBadSessRefInSessionMsg = -206009,
	SQLSingleRowReturnFailed = -206010
}
