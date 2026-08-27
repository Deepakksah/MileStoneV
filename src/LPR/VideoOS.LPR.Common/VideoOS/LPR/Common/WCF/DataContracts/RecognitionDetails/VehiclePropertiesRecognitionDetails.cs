using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class VehiclePropertiesRecognitionDetails : IExtensibleDataObject
{
	[DataMember]
	public float? ConfidenceType { get; set; }

	[DataMember]
	public string ValueType { get; set; }

	[DataMember]
	public float? ConfidenceColor { get; set; }

	[DataMember]
	public string ValueColor { get; set; }

	[DataMember]
	public float? ConfidenceMake { get; set; }

	[DataMember]
	public string ValueMake { get; set; }

	[DataMember]
	public float? ConfidenceModel { get; set; }

	[DataMember]
	public string ValueModel { get; set; }

	[DataMember]
	public float? ConfidenceSide { get; set; }

	[DataMember]
	public string ValueSide { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
