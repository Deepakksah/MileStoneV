using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetAccessControlUnitPositionParams", Namespace = "urn:milestone-systems")]
public class SetAccessControlUnitPositionParams : IExtensibleDataObject
{
	[DataMember]
	public Guid AccessControlSystemId { get; set; }

	[DataMember]
	public Guid AccessControlUnitId { get; set; }

	[DataMember]
	public double Latitude { get; set; }

	[DataMember]
	public double Longitude { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
