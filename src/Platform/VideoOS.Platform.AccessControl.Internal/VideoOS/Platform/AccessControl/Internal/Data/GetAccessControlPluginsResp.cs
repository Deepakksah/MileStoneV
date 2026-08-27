using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPluginsResp", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginsResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlPlugin[] AccessControlPlugins { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
