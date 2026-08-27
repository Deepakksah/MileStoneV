using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("Change: Id={Id}, Time={Time}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Change
{
	[DataMember]
	public ChangeType ChangeType { get; set; }

	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public DateTime Time { get; set; }
}
