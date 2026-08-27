using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("PresetInfo: {Name} ({Id})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PresetInfo
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public PositionInfo Position { get; set; }

	[DataMember]
	public string Shortcut { get; set; }

	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public bool Locked { get; set; }

	public PresetInfo()
	{
		Position = new PositionInfo();
	}

	public PresetInfo(string presetName)
	{
		Name = presetName;
		Position = new PositionInfo();
	}
}
