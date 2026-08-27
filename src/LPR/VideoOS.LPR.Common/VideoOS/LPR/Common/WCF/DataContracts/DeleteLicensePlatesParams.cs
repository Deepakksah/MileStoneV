using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class DeleteLicensePlatesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ListId { get; set; }

	[DataMember]
	public LicensePlate[] LicensePlates { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
