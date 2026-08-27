using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("DeviceDisabledInfo: {DeviceName} ({DeviceId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class DeviceDisabledInfo
{
	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public Guid HardwareId { get; set; }

	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public string DeviceType { get; set; }

	[DataMember]
	public string DeviceName { get; set; }
}
