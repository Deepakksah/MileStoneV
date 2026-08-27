using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class PatchCountryModulesLicensePlateStylesParams : IExtensibleDataObject
{
	[DataMember]
	public CountryModuleWithLicensePlateStyles[] CountryModulesWithLicensePlateStyles { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
