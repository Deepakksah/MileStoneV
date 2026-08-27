using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class AddLicensePlatesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ListId { get; set; }

	[DataMember]
	[Obsolete("Use LicensePlateInfos field instead")]
	public LicensePlate[] LicensePlates { get; set; }

	[DataMember]
	public LicensePlateInfo[] LicensePlateInfos { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
