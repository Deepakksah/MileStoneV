using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetCountryModulesWithLicensePlateStylesResp : IExtensibleDataObject
{
	[DataMember]
	public CountryModuleWithLicensePlateStyles[] CountryModuleWithLicensePlateStyles { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
