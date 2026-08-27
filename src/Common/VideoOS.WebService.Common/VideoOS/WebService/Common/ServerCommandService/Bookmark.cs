using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("Bookmark: {Header} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Bookmark
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public string Reference { get; set; }

	[DataMember]
	public string Header { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string User { get; set; }

	[DataMember]
	public DateTime TimeBegin { get; set; }

	[DataMember]
	public DateTime TimeTrigged { get; set; }

	[DataMember]
	public DateTime TimeEnd { get; set; }
}
