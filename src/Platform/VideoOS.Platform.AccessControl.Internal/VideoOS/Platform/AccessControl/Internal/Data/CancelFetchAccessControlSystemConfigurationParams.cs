using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "CancelFetchAccessControlSystemConfigurationParams", Namespace = "urn:milestone-systems")]
public class CancelFetchAccessControlSystemConfigurationParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
