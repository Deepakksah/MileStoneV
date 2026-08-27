using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SearchCredentialHoldersResp", Namespace = "urn:milestone-systems")]
public class SearchCredentialHoldersResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public AccessControlCredentialHolderSearchResult[] SearchResults { get; set; }

	[DataMember]
	public bool PartialResult { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
