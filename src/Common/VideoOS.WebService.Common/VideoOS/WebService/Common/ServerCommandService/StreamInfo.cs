using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class StreamInfo
{
	[DataMember]
	public Guid StreamId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public bool Default { get; set; }
}
