using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchGroup : IExtensibleDataObject
{
	[DataMember]
	public SearchElement[] SearchElements { get; set; }

	[DataMember]
	public SearchGroup[] SearchGroups { get; set; }

	[DataMember]
	public LogicalOperator GroupOperator { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
