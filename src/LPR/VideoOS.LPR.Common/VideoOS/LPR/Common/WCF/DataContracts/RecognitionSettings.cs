using System.Drawing;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class RecognitionSettings : IExtensibleDataObject
{
	[DataMember]
	public RectangleF? Area { get; set; }

	[DataMember]
	public byte[] Mask { get; set; }

	[DataMember]
	public Size? MaskSize { get; set; }

	[DataMember]
	public float? MinHeight { get; set; }

	[DataMember]
	public float? MaxHeight { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
