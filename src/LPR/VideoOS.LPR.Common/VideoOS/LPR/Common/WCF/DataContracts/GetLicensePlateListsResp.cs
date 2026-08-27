using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetLicensePlateListsResp : IExtensibleDataObject
{
	[DataMember]
	public LicensePlateList[] LicensePlateLists { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
