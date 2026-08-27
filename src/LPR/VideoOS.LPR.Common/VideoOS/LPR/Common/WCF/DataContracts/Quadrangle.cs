using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class Quadrangle : IExtensibleDataObject
{
	[DataMember]
	public Point UpperLeft { get; set; }

	[DataMember]
	public Point UpperRight { get; set; }

	[DataMember]
	public Point LowerLeft { get; set; }

	[DataMember]
	public Point LowerRight { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
