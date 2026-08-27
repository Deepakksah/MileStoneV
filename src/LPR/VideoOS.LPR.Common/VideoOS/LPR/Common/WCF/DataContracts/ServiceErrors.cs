using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
[Flags]
public enum ServiceErrors
{
	[EnumMember]
	None = 0,
	[EnumMember]
	UnspecifiedError = 1,
	[EnumMember]
	SurveillanceSystemConnectionError = 2,
	[EnumMember]
	WebServiceError = 4,
	[EnumMember]
	LicensePlatesPollError = 8,
	[EnumMember]
	RunningOnOtherSystem = 0x10
}
