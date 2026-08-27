using System;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class HardwareStateChangedEventArgs : StatusApiEventArgs
{
	private readonly Guid _hardwareId;

	private readonly bool _enabled;

	private readonly bool _started;

	private readonly bool _isChange;

	private readonly bool _error;

	private readonly bool _errorNoConnection;

	private readonly bool _errorNotLicensed;

	public Guid HardwareId => _hardwareId;

	public bool Enabled => _enabled;

	public bool Started => _started;

	public bool IsChange => _isChange;

	public bool Error => _error;

	public bool ErrorNoConnection => _errorNoConnection;

	public bool ErrorNotLicensed => _errorNotLicensed;

	public HardwareStateChangedEventArgs(DateTime time, Guid hardwareId, bool enabled, bool started, bool isChange, bool error, bool errorNoConnection, bool errorNotLicensed)
		: base(time)
	{
		_hardwareId = hardwareId;
		_enabled = enabled;
		_started = started;
		_isChange = isChange;
		_error = error;
		_errorNoConnection = errorNoConnection;
		_errorNotLicensed = errorNotLicensed;
	}
}
