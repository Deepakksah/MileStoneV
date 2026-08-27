using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlate : IExtensibleDataObject
{
	[DataMember]
	public string RegistrationNumber { get; set; }

	[DataMember]
	public MatchListItemType MatchListItemType { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
