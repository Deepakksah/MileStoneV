using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetCredentialHolderImageResp", Namespace = "urn:milestone-systems")]
public class SetCredentialHolderImageResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
