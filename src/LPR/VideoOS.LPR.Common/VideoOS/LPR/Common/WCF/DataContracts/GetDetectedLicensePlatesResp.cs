using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetDetectedLicensePlatesResp : IExtensibleDataObject
{
	[DataMember]
	public DetectedLicensePlate[] DetectedLicensePlates { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public GetDetectedLicensePlatesResp()
	{
		DetectedLicensePlates = Array.Empty<DetectedLicensePlate>();
	}
}
