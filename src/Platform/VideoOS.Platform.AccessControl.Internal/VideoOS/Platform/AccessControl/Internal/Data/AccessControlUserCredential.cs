using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlUserCredential", Namespace = "urn:milestone-systems")]
public class AccessControlUserCredential : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public string Username { get; set; }

	[DataMember]
	public string Password { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
