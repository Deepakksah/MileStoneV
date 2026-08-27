using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlateInfo : IExtensibleDataObject
{
	[DataMember]
	public LicensePlate LicensePlate { get; set; }

	[DataMember]
	public LicensePlateProperty[] Properties { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
