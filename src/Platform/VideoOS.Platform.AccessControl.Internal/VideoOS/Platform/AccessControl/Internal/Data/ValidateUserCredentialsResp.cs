using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ValidateUserCredentialsResp", Namespace = "urn:milestone-systems")]
public class ValidateUserCredentialsResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlUserCredentialValidationResult[] AccessControlUserCredentialValidationResults { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
