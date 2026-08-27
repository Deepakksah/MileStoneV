using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPluginsParams", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginsParams : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
