using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class RecognitionResult : IExtensibleDataObject
{
	[DataMember]
	public TimeSpan? RecognitionTime { get; set; }

	[DataMember]
	public float? NoPlatesConfidence { get; set; }

	[DataMember]
	public PlateRecognitionDetails[] Plates { get; set; }

	[DataMember]
	public AreaRecognitionDetails[] Areas { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
