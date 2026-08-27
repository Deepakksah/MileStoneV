using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetDowntimesParams", Namespace = "urn:milestone-systems")]
public class GetDowntimesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid[] Ids { get; set; }

	[DataMember]
	public DateTime Start { get; set; }

	[DataMember]
	public DateTime End { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
