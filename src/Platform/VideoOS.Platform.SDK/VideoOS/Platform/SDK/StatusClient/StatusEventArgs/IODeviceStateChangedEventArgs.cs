using System;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class IODeviceStateChangedEventArgs : DeviceStateChangedEventArgs
{
	private readonly string _ioState;

	public string IoState => _ioState;

	public IODeviceStateChangedEventArgs(DateTime time, Guid deviceId, bool enabled, bool started, bool isChange, bool error, bool errorNoConnection, bool errorNotLicensed, string ioState)
		: base(time, deviceId, enabled, started, isChange, error, errorNoConnection, errorNotLicensed)
	{
		_ioState = ioState;
	}
}
