using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetAccessControlUnitPositionResp", Namespace = "urn:milestone-systems")]
public class SetAccessControlUnitPositionResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
