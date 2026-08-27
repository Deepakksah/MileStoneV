using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MetadataType
{
	private readonly IMetadataTypeInfo _info;

	public Guid DisplayId => _info.DisplayId;

	public string DisplayName => _info.DisplayName;

	public Guid Id => _info.Id;

	public string Name => _info.Name;

	public int ValidTime => _info.ValidTime;

	public MetadataType(IMetadataTypeInfo info)
	{
		_info = info;
	}
}
