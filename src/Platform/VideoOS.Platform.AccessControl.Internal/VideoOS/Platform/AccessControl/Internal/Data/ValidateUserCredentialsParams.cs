using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ValidateUserCredentialsParams", Namespace = "urn:milestone-systems")]
public class ValidateUserCredentialsParams : IExtensibleDataObject
{
	[DataMember]
	public AccessControlUserCredential[] AccessControlUserCredentials { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
