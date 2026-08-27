using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract(Name = "PluginEventInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class PluginEventInfo
{
	[DataMember]
	public Guid PluginEventId { get; set; }

	[DataMember]
	public bool IsDeviceEvent { get; set; }

	[DataMember]
	public Guid SourceId { get; set; }

	[DataMember]
	public Guid[] MetadataDeviceIds { get; set; }

	[DataMember]
	public Guid[] MetadataDeviceGroupIds { get; set; }

	[DataMember]
	public string CustomData { get; set; }
}
