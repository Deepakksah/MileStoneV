using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "CancelFetchAccessControlSystemConfigurationResp", Namespace = "urn:milestone-systems")]
public class CancelFetchAccessControlSystemConfigurationResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
