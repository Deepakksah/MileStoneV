using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlPlugincategoriesParams", Namespace = "urn:milestone-systems")]
public class GetAccessControlPluginCategoriesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
