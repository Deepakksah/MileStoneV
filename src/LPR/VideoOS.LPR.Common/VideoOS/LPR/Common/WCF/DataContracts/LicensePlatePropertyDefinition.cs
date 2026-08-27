using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlatePropertyDefinition : IExtensibleDataObject
{
	[DataMember]
	public int ColumnIndex { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public int DisplayOrder { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
