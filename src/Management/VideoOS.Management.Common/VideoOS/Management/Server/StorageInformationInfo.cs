using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "StorageInformationInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class StorageInformationInfo
{
	[DataMember]
	public StorageDiskUsageInfo DiskUsage{ get; set; }
}
