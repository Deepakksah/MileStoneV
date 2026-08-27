using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlGlobalSettings", Namespace = "urn:milestone-systems")]
public class AccessControlGlobalSettings : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public bool ShowDevelopmentOptions { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
