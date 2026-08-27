using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "StartFetchAccessControlSystemConfigurationResp", Namespace = "urn:milestone-systems")]
public class StartFetchAccessControlSystemConfigurationResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
