using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetVideoSourcesResp : IExtensibleDataObject
{
	[DataMember]
	public string[] VideoSources { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
