using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("DeviceMappingInfo: {DeviceA}, {DeviceB}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class DeviceMappingInfo
{
	[DataMember]
	public Guid DeviceA { get; set; }

	[DataMember]
	public Guid DeviceB { get; set; }
}
