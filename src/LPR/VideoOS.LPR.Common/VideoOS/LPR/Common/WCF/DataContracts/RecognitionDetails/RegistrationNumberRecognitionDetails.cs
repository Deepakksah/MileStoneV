using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class RegistrationNumberRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? Confidence { get; set; }

	[DataMember]
	public string Value { get; set; }

	[DataMember]
	public CharacterRecognitionDetails[] Characters { get; set; }

	[DataMember]
	public string RegistrationNumberSymbols { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
