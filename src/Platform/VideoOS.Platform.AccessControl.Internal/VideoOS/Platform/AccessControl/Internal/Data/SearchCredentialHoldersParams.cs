using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SearchCredentialHoldersParams", Namespace = "urn:milestone-systems")]
public class SearchCredentialHoldersParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public string SearchString { get; set; }

	[DataMember]
	public int SearchLimit { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
