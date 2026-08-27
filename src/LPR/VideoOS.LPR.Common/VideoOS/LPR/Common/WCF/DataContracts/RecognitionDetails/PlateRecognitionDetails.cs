using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class PlateRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? Confidence { get; set; }

	[DataMember]
	public CountryRecognitionDetails Country { get; set; }

	[DataMember]
	public RegistrationNumberRecognitionDetails RegistrationNumber { get; set; }

	[DataMember]
	public float CharacterHeight { get; set; }

	[DataMember]
	public PlateStyleRecognitionDetails PlateStyle { get; set; }

	[DataMember]
	public Quadrangle Location { get; set; }

	[DataMember]
	public VehiclePropertiesRecognitionDetails VehicleProperties { get; set; }

	[DataMember]
	public PlateRecognitionDetails AlternativePlate { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
