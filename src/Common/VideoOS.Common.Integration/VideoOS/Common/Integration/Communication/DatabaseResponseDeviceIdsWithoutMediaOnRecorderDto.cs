using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class DatabaseResponseDeviceIdsWithoutMediaOnRecorderDto : DatabaseResponseDto
{
	public Guid[] DeviceIds { get; private set; }
}
