using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetAccessControlSystemConfigurationParams", Namespace = "urn:milestone-systems")]
public class SetAccessControlSystemConfigurationParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public string SerializedConfiguration { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
