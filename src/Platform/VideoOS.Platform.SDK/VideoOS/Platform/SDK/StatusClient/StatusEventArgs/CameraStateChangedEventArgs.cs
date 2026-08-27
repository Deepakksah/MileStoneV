using System;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class CameraStateChangedEventArgs : MediaStreamDeviceStateChangedEventArgs
{
	private readonly bool _motion;

	public bool Motion => _motion;

	public CameraStateChangedEventArgs(DateTime time, Guid deviceId, bool enabled, bool started, bool isChange, bool error, bool errorNoConnection, bool errorNotLicensed, bool recording, bool dbMoveInProgress, bool errorOverflow, bool errorWritingGop, bool dbRepairInProgress, bool motion)
		: base(time, deviceId, enabled, started, isChange, error, errorNoConnection, errorNotLicensed, recording, dbMoveInProgress, errorOverflow, errorWritingGop, dbRepairInProgress)
	{
		_motion = motion;
	}
}
