using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CustomSettingInfo
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public DateTime LastModifiedTime { get; set; }

	[DataMember]
	public string LastModifiedUser { get; set; }
}
