using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ExecuteCommandParams", Namespace = "urn:milestone-systems")]
public class ExecuteCommandParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public Guid AccessControlOperationableInstanceId { get; set; }

	[DataMember]
	public Guid AccessControlCommandTypeId { get; set; }

	[DataMember]
	public string Culture { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
