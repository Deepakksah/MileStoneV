using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

public class MetadataTypeInfo : IMetadataTypeInfo
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public Guid DisplayId { get; set; }

	public string DisplayName { get; set; }

	public int ValidTime { get; set; }
}
