using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlateLookupResultGroup : IExtensibleDataObject
{
	[DataMember]
	public LicensePlateList LicensePlateList { get; set; }

	[DataMember]
	public LicensePlateInfo[] LicensePlateInfos { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
