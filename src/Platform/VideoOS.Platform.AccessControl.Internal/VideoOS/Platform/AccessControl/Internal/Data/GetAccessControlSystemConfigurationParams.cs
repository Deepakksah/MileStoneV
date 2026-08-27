using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlSystemConfigurationParams", Namespace = "urn:milestone-systems")]
public class GetAccessControlSystemConfigurationParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public bool BypassFilteringBasedOnRole { get; set; }

	public AccessControlUserCredential accessControlUserCredential { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
