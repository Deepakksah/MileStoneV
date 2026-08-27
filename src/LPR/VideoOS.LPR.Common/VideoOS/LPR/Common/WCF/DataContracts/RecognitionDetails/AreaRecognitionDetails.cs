using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class AreaRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? Confidence { get; set; }

	[DataMember]
	public Quadrangle Location { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
