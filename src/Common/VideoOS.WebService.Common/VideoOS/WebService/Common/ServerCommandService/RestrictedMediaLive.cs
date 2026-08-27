using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("RestrictedMediaLive: {DeviceId}, ({StartTime})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaLive
{
	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public string UserName { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public DateTime StartTime { get; set; }
}
