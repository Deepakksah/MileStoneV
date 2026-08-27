using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchOrder : IExtensibleDataObject
{
	[DataMember]
	public string Target { get; set; }

	[DataMember]
	public Direction Direction { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
