using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetCountryModulesWithLicensePlateStylesParams : IExtensibleDataObject
{
	[DataMember]
	public string[] CountryModuleCodes { get; set; } = Array.Empty<string>();

	public ExtensionDataObject ExtensionData { get; set; }
}
