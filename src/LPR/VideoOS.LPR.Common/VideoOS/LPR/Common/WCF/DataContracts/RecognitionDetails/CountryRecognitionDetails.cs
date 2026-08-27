using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class CountryRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? Confidence { get; set; }

	[DataMember]
	public string CountryCode { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
