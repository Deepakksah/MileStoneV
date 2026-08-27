using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlateProperty : IExtensibleDataObject
{
	[DataMember]
	public int ColumnIndex { get; set; }

	[DataMember]
	public string Value { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
