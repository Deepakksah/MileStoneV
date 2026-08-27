using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class MoveDeviceDto
{
	public Guid DeviceId { get; private set; }

	public Guid SourceStorageId { get; private set; }

	public Guid NewStorageId { get; private set; }

	public MoveDeviceSteps Step { get; set; }

	public MoveDeviceDto(Guid deviceId, Guid sourceStorageId, Guid newStorageId, MoveDeviceSteps step)
	{
		DeviceId = deviceId;
		SourceStorageId = sourceStorageId;
		NewStorageId = newStorageId;
		Step = step;
	}
}
