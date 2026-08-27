using System;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "CreateAccessControlSystemParams", Namespace = "urn:milestone-systems")]
public class CreateAccessControlSystemParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public Property[] Properties { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
