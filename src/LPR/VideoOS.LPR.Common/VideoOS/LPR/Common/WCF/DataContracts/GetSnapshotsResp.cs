using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetSnapshotsResp : IExtensibleDataObject
{
	[DataMember]
	public SnapshotData[] Snapshots { get; set; }

	[DataMember]
	public bool More { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
