using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetGlobalSettingsParams", Namespace = "urn:milestone-systems")]
public class SetGlobalSettingsParams : IExtensibleDataObject
{
	[DataMember]
	public AccessControlGlobalSettings GlobalSettings { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
