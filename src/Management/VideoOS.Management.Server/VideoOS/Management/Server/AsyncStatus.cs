using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "AsyncStatusInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class AsyncStatus
{
	[DataMember]
	public string LastException { get; set; }

	[DataMember]
	public ExceptionStatus ExceptionStatus { get; set; }
}
