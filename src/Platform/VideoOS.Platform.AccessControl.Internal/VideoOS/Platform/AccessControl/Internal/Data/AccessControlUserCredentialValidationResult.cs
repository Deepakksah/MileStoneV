using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlUserCredentialValidationResult", Namespace = "urn:milestone-systems")]
public class AccessControlUserCredentialValidationResult : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public bool CredentialsValid { get; set; }

	[DataMember]
	public string ErrorMessage { get; set; }

	[DataMember]
	public int ErrorCode { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
