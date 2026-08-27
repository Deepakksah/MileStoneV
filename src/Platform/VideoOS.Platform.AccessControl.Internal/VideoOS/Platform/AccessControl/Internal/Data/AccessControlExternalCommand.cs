using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlExternalCommand", Namespace = "urn:milestone-systems")]
public class AccessControlExternalCommand : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public string Label { get; set; }

	[DataMember(Order = 2)]
	public string Command { get; set; }

	[DataMember(Order = 3)]
	public string Type { get; set; }

	[DataMember(Order = 4)]
	public int Order { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlExternalCommand()
	{
	}

	public AccessControlExternalCommand(string label, string command, string type)
	{
		Label = label;
		Command = command;
		Type = type;
	}
}
