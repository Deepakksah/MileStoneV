using System;
using System.Runtime.Serialization;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SearchLicensePlateEventsResp : IExtensibleDataObject
{
	[DataMember]
	public Guid SearchId { get; set; }

	[DataMember]
	public EventLine[] Result { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
