using System;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class MediaStreamDeviceStateChangedEventArgs : DeviceStateChangedEventArgs
{
	private readonly bool _recording;

	private readonly bool _dbMoveInProgress;

	private readonly bool _errorOverflow;

	private readonly bool _errorWritingGop;

	private readonly bool _dbRepairInProgress;

	public bool Recording => _recording;

	public bool DbMoveInProgress => _dbMoveInProgress;

	public bool ErrorOverflow => _errorOverflow;

	public bool ErrorWritingGop => _errorWritingGop;

	public bool DbRepairInProgress => _dbRepairInProgress;

	public MediaStreamDeviceStateChangedEventArgs(DateTime time, Guid deviceId, bool enabled, bool started, bool isChange, bool error, bool errorNoConnection, bool errorNotLicensed, bool recording, bool dbMoveInProgress, bool errorOverflow, bool errorWritingGop, bool dbRepairInProgress)
		: base(time, deviceId, enabled, started, isChange, error, errorNoConnection, errorNotLicensed)
	{
		_recording = recording;
		_dbMoveInProgress = dbMoveInProgress;
		_errorOverflow = errorOverflow;
		_errorWritingGop = errorWritingGop;
		_dbRepairInProgress = dbRepairInProgress;
	}
}
