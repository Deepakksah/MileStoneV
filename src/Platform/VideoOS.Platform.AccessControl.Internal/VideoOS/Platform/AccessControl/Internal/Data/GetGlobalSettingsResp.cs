using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetGlobalSettingsResp", Namespace = "urn:milestone-systems")]
public class GetGlobalSettingsResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlGlobalSettings GlobalSettings { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
