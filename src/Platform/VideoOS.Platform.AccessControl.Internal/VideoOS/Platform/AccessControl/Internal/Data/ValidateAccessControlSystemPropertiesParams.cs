using System;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ValidateAccessControlSystemPropertiesParams", Namespace = "urn:milestone-systems")]
public class ValidateAccessControlSystemPropertiesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public Property[] Properties { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
