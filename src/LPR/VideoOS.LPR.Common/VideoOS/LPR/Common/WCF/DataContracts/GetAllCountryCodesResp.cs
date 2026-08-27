using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetAllCountryCodesResp : IExtensibleDataObject
{
	[DataMember]
	public string[] CountryCodes { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
