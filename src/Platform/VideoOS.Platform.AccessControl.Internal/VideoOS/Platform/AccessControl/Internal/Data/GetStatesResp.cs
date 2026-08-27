using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetStatesResp", Namespace = "urn:milestone-systems")]
public class GetStatesResp : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public AccessControlState[] AccessControlStates { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
