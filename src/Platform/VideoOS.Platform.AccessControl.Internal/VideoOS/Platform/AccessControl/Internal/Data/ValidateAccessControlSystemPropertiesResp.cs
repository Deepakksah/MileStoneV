using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ValidateAccessControlSystemPropertiesResp", Namespace = "urn:milestone-systems")]
public class ValidateAccessControlSystemPropertiesResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public AccessControlPropertyValidationResult PropertyValidationResult { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
