using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class VideoSourceStatus : IExtensibleDataObject
{
	[DataMember]
	public Guid Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public float FrameRate { get; set; }

	[DataMember]
	public VideoSourceError Errors { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
