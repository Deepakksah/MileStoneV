using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchElement : IExtensibleDataObject
{
	[DataMember]
	public string SearchTarget { get; set; }

	[DataMember]
	public SearchOperator SearchOperator { get; set; }

	[DataMember]
	public object SearchValue { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
