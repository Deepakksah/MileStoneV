using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertOccurrence
{
	[DataMember]
	public Guid AlertId { get; set; }

	[DataMember]
	public DateTime Time { get; set; }
}
