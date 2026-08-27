using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("CapabilityInfo: {Name}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class CapabilityInfo
{
	[DataMember]
	public Guid CapabilityId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public bool Absolute { get; set; }

	[DataMember]
	public bool Relative { get; set; }

	[DataMember]
	public bool Start { get; set; }

	[DataMember]
	public bool Stop { get; set; }

	[DataMember]
	public bool Speed { get; set; }

	[DataMember]
	public bool Automatic { get; set; }

	public CapabilityInfo()
	{
	}

	public CapabilityInfo(string name)
	{
		Name = name;
	}
}
