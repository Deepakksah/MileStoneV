using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class Point : IExtensibleDataObject
{
	[DataMember]
	public float X { get; set; }

	[DataMember]
	public float Y { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
