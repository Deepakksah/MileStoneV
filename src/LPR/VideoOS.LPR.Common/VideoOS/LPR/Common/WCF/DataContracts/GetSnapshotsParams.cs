using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetSnapshotsParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ManagementServerId { get; set; }

	[DataMember]
	public Guid LprDeviceId { get; set; }

	[DataMember]
	public int StartIndex { get; set; }

	[DataMember]
	public int Count { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
