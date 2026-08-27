using System;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[Serializable]
public enum DiscoveryOutputParametersError
{
	NoUser,
	NoDefaultPassword,
	ResultTooLong,
	GetInfo2NotSupported,
	HttpsPortInParameterNotSupported,
	Unknown
}
