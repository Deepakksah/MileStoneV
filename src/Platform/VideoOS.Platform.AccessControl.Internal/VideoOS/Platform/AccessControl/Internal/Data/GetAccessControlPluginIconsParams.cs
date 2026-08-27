using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPluginIconsParams", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginIconsParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
