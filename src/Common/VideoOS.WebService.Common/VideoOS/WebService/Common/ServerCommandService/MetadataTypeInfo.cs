using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class MetadataTypeInfo
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public Guid DisplayId { get; set; }

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public int ValidTime { get; set; }
}
