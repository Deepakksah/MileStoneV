using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchOrders : IExtensibleDataObject
{
	[DataMember]
	public SearchOrder[] Orders { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
