using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetAccessControlSystemConfigurationResp", Namespace = "urn:milestone-systems")]
public class SetAccessControlSystemConfigurationResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
