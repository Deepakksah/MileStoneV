using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetStatusResp : IExtensibleDataObject
{
	[DataMember]
	public ServiceErrors Errors { get; set; }

	[DataMember]
	public ServiceStatus Status { get; set; }

	[DataMember]
	public VideoSourceStatus[] VideoSourceStatuses { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public GetStatusResp()
	{
		VideoSourceStatuses = Array.Empty<VideoSourceStatus>();
	}
}
