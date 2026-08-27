using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public enum ErrorCode
{
	[EnumMember]
	UnknownError,
	[EnumMember]
	AuthenticationError,
	[EnumMember]
	EventServerNotRegistered,
	[EnumMember]
	LPRServerNotRegistered,
	[EnumMember]
	ImageServerCommunicationError,
	[EnumMember]
	EventServerCommunicationError,
	[EnumMember]
	LPRServerCommunicationError,
	[EnumMember]
	LPRServerRunningOnOtherSystem,
	[EnumMember]
	InvalidParameters
}
