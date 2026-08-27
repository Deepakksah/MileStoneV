using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum RecorderWebInterfaceErrorCode
{
	None = 0,
	GeneralError = 10000,
	SoapVersionMismatchError = 10001,
	InvalidSoapError = 10002,
	UnknownSoapVersionError = 10003,
	TokenError = 20001,
	MediaNotReady = 30001,
	MethodHandlingError = 40000,
	PtzError = 40295
}
