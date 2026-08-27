using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class ModifyLicensePlateListResp : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
