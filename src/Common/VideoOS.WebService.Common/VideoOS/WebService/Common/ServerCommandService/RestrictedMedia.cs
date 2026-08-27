using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("RestrictedMedia: {Header}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMedia
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public Guid[] DeviceIds { get; set; }

	[DataMember]
	public string Header { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string UserName { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public DateTime StartTime { get; set; }

	[DataMember]
	public DateTime EndTime { get; set; }
}
