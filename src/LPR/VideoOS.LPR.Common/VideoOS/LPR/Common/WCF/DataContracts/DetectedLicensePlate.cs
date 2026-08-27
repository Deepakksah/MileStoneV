using System;
using System.Runtime.Serialization;
using VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class DetectedLicensePlate : IExtensibleDataObject
{
	[DataMember]
	public FQID VideoSourceId { get; set; }

	[DataMember]
	public FQID CameraId { get; set; }

	[DataMember]
	public string CameraName { get; set; }

	[DataMember]
	public DateTime Time { get; set; }

	[DataMember]
	public PlateRecognitionDetails LicensePlate { get; set; }

	[DataMember]
	public LicensePlateList[] LicensePlateListMatches { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
