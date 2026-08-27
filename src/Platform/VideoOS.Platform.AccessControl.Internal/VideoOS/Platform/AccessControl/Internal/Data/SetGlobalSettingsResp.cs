using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetGlobalSettingsResp", Namespace = "urn:milestone-systems")]
public class SetGlobalSettingsResp : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
