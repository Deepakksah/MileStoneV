using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class CharacterRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? Confidence { get; set; }

	[DataMember]
	public char Value { get; set; }

	[DataMember]
	public Quadrangle Location { get; set; }

	[DataMember]
	public CharacterRecognitionDetails AlternateCharacter { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
