using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("ViewGroupInfo: {Name} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ViewGroupInfo
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public ViewGroupType ViewGroupType { get; set; }

	[DataMember]
	public bool ReadOnly { get; set; }

	[DataMember]
	public DateTime LastModifiedTime { get; set; }

	[DataMember]
	public string LastModifiedUser { get; set; }
}
