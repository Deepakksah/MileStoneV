using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetCredentialHoldersParams", Namespace = "urn:milestone-systems")]
public class GetCredentialHoldersParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public string[] AccessControlCredentialHolderIds { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
