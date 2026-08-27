using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LookupLicensePlateInfosParams : IExtensibleDataObject
{
	[DataMember]
	public LicensePlateLookupGroup[] LicensePlateLookups { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
