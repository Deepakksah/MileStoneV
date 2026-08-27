using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPluginIconsResp", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginIconsResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public AccessControlIcon[] PluginIcons { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
