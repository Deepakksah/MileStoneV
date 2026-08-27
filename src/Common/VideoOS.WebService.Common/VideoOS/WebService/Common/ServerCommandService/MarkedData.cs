using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("MarkedData: {Header}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MarkedData
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public Guid[] DeviceIds { get; set; }

	[DataMember]
	public DateTime Created { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public string Reference { get; set; }

	[DataMember]
	public string Header { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string User { get; set; }

	[DataMember]
	public DateTime StartTime { get; set; }

	[DataMember]
	public DateTime TagTime { get; set; }

	[DataMember]
	public DateTime EndTime { get; set; }

	[DataMember]
	public bool UseRetention { get; set; }

	[DataMember]
	public DateTime RetentionExpire { get; set; }

	[DataMember]
	public long RetentionSize { get; set; }

	[DataMember]
	public RetentionOption RetentionOption { get; set; }

	public MarkedData()
	{
		RetentionOption = new RetentionOption();
	}
}
