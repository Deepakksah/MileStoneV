using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlPropertyDefinition", Namespace = "urn:milestone-systems")]
public class AccessControlPropertyDefinition : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public string DisplayName { get; set; }

	[DataMember(Order = 2)]
	public string Key { get; set; }

	[DataMember(Order = 3)]
	public string DefaultValue { get; set; }

	[DataMember(Order = 4)]
	public int Importance { get; set; }

	[DataMember(Order = 6)]
	public string ToolTip { get; set; }

	[DataMember(Order = 7)]
	public string ValueType { get; set; }

	[DataMember(Order = 8)]
	public Property[] ValueTypeProperties { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
