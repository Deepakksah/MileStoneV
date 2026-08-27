using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetStatesParams", Namespace = "urn:milestone-systems")]
public class GetStatesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public Guid[] AccessControlOperationableInstanceIds { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
