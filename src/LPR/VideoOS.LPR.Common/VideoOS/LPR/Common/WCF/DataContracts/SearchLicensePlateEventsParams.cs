using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchLicensePlateEventsParams : IExtensibleDataObject
{
	[DataMember]
	public Guid SearchId { get; set; }

	[DataMember]
	public SearchGroup SearchGroup { get; set; }

	[DataMember]
	public SearchOrders SearchOrders { get; set; }

	[DataMember]
	public int Page { get; set; }

	[DataMember]
	public int PageSize { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
