using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class AnalyzeJpegParams : IExtensibleDataObject
{
	[DataMember]
	public byte[] JpegBytes { get; set; }

	[DataMember]
	public Guid ManagementServerId { get; set; }

	[DataMember]
	public IList<string> SelectedCountryCodes { get; set; }

	[DataMember]
	public RecognitionSettings RecognitionSettings { get; set; }

	[DataMember]
	public StoppingCriterion StoppingCriterion { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
