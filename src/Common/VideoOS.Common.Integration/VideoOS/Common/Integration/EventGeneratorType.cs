using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
[GeneratedCode("CreateLogUserEnumerations build task", "0.0.0.0")]
public enum EventGeneratorType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	System,
	[EnumMember]
	RecorderPlugin,
	[EnumMember]
	Device,
	[EnumMember]
	Recorder,
	[EnumMember]
	Server,
	[EnumMember]
	Timer,
	[EnumMember]
	External,
	[EnumMember]
	DriverDevice,
	[EnumMember]
	DriverHardware,
	[EnumMember]
	Hardware,
	[EnumMember]
	MIP,
	[EnumMember]
	MIPDevice,
	[EnumMember]
	SystemDashboard
}
