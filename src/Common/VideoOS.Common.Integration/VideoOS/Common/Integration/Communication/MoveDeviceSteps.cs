using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public enum MoveDeviceSteps
{
	[EnumMember]
	MoveDevice,
	[EnumMember]
	MoveData,
	[EnumMember]
	CanMoveData
}
