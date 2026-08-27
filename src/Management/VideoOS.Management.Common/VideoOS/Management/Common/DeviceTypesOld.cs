using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract(Name = "DeviceTypes", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
[Flags]
public enum DeviceTypesOld
{
	[EnumMember]
	Unknown = 0,
	[EnumMember]
	Camera = 1,
	[EnumMember]
	Input = 2,
	[EnumMember]
	Output = 4,
	[EnumMember]
	Microphone = 8,
	[EnumMember]
	Speaker = 0x10,
	[EnumMember]
	AllAudio = 0x18,
	[EnumMember]
	All = 0x7F
}
