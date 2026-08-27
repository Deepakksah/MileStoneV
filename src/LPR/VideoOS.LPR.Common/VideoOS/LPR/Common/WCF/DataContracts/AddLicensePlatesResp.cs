using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class AddLicensePlatesResp : IExtensibleDataObject
{
	[DataMember]
	public int Count { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
