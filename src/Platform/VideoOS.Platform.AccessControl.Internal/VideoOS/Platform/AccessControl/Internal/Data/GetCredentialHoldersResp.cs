using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetCredentialHoldersResp", Namespace = "urn:milestone-systems")]
public class GetCredentialHoldersResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public AccessControlCredentialHolder[] CredentialHolders { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
