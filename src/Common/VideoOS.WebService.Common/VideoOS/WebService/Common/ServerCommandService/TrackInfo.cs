using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class TrackInfo
{
	[DataMember]
	public Guid TrackId { get; set; }

	[DataMember]
	public bool Edge { get; set; }
}
