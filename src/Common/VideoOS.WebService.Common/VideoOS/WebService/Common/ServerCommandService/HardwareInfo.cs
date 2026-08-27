using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("HardwareInfo: {Name} ({HardwareId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class HardwareInfo
{
	[DataMember]
	public Guid HardwareId { get; set; }

	[DataMember]
	public Guid[] DeviceIds { get; set; }

	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string Uri { get; set; }

	[DataMember]
	public bool Interconnected { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }
}
