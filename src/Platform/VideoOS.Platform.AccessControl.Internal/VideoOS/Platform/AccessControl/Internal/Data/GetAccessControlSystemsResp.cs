using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlSystemsResp", Namespace = "urn:milestone-systems")]
public class GetAccessControlSystemsResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlPluginId { get; set; }

	[DataMember]
	public AccessControlSystem[] AccessControlSystems { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
