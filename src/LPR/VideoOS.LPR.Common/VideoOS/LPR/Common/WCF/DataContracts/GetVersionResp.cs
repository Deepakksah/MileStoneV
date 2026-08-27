using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetVersionResp : IExtensibleDataObject
{
	[DataMember]
	public string Version { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
