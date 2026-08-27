using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlCredentialHolderSearchResult", Namespace = "urn:milestone-systems")]
public class AccessControlCredentialHolderSearchResult : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid AccessControlSystemId { get; set; }

	[DataMember(Order = 2)]
	public string AccessControlCredentialHolderId { get; set; }

	[DataMember(Order = 3)]
	public string[] CredentialHolderRoles { get; set; }

	[DataMember(Order = 4)]
	public string Name { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlCredentialHolderSearchResult()
	{
	}

	public AccessControlCredentialHolderSearchResult(Guid accessControlSystemId, string accessControlCredentialHolderId, string name, string[] credentialHolderRoles)
	{
		AccessControlSystemId = accessControlSystemId;
		AccessControlCredentialHolderId = accessControlCredentialHolderId;
		CredentialHolderRoles = credentialHolderRoles;
		Name = name;
	}
}
