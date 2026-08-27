using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetGlobalSettingsParams", Namespace = "urn:milestone-systems")]
public class GetGlobalSettingsParams : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
