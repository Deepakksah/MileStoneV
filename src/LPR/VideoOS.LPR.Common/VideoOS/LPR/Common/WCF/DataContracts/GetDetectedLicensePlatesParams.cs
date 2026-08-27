using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetDetectedLicensePlatesParams : IExtensibleDataObject
{
	[DataMember]
	public TimeSpan CallHangTime { get; set; }

	[DataMember]
	public Guid ManagementServerId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
