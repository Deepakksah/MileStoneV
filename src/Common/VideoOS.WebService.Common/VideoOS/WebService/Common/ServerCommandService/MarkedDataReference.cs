using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("MarkedDataReference: {Reference}, {Time}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MarkedDataReference
{
	[DataMember]
	public string Reference { get; set; }

	[DataMember]
	public DateTime Time { get; set; }
}
