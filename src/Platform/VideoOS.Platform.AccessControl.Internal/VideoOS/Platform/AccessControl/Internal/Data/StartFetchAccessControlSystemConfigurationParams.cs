using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "StartFetchAccessControlSystemConfigurationParams", Namespace = "urn:milestone-systems")]
public class StartFetchAccessControlSystemConfigurationParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
