using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class StoppingCriterion : IExtensibleDataObject
{
	[DataMember]
	public int? TimeLimit { get; set; }

	[DataMember]
	public float? MinConfidence { get; set; }

	[DataMember]
	public int? MaxPlates { get; set; }

	[DataMember]
	public float? RejectConfidence { get; set; }

	[DataMember]
	public float? FrameRate { get; set; }

	[DataMember]
	public bool? Interlaced { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
