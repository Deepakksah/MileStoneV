using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "StorageDiskUsageInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class StorageDiskUsageInfo
{
	[DataMember]
	public long AvailableSpace { get; set; }

	[DataMember]
	public long UsedSpace { get; set; }

	[DataMember]
	public long LockedUsedSpace { get; set; }

	[DataMember]
	public DateTimeOffset LastUpdated{ get; set; }
}
