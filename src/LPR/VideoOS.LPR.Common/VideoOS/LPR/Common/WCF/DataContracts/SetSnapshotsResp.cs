using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class SetSnapshotsResp : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
