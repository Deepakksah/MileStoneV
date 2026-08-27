using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPluginCategoriesResp", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginCategoriesResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public AccessControlPluginCategory[] AccessControlPluginCategories { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
