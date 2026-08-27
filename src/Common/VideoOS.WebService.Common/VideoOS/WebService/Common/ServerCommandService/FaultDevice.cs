using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class FaultDevice
{
	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public int ErrorCode { get; set; }

	[DataMember]
	public string Message { get; set; }
}
