using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetIntradaCountryModulesResp : IExtensibleDataObject
{
	[DataMember]
	public string[] CountryModules { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public GetIntradaCountryModulesResp()
	{
		CountryModules = Array.Empty<string>();
	}
}
