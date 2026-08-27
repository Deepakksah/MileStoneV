using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class CountryModuleWithLicensePlateStyles : IExtensibleDataObject
{
	[DataMember]
	public string Code { get; set; }

	[DataMember]
	public LicensePlateStyle[] LicensePlateStyles { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
