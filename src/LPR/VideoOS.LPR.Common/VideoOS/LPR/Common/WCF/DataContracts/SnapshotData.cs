using System.Runtime.Serialization;
using VideoOS.LPR.Common.WCF.DataContracts.RecognitionDetails;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SnapshotData : IExtensibleDataObject
{
	[DataMember]
	public byte[] JpegBytes { get; set; }

	[DataMember]
	public RecognitionResult RecognitionResult { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
