using System;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK;

public sealed class Capability
{
	private ICapabilityInfo _capabilityInfo;

	public Guid CapabilityId => _capabilityInfo.CapabilityId;

	public string Name => _capabilityInfo.Name;

	public bool Absolute => _capabilityInfo.Absolute;

	public bool Relative
	{
		get
		{
			if (!_capabilityInfo.Relative)
			{
				return _capabilityInfo.Absolute;
			}
			return true;
		}
	}

	public bool Start => _capabilityInfo.Start;

	public bool Stop => _capabilityInfo.Stop;

	public bool Speed => _capabilityInfo.Speed;

	public bool Automatic => _capabilityInfo.Automatic;

	public Capability(ICapabilityInfo capabilityInfo)
	{
		_capabilityInfo = capabilityInfo;
	}
}
