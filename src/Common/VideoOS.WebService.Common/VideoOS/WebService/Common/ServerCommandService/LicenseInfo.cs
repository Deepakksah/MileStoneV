using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class LicenseInfo
{
	[DataMember]
	public Guid LicenseId { get; set; }

	[DataMember]
	public string Data { get; set; }
}
