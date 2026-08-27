using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetLicensePlatesResp : IExtensibleDataObject
{
	[DataMember]
	[Obsolete("Kept for backward compatibility. Use LicensePlateInfos instead.")]
	public LicensePlate[] LicensePlates { get; set; }

	[DataMember]
	public LicensePlateInfo[] LicensePlateInfos { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
