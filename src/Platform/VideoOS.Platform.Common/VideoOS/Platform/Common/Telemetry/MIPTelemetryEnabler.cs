using System;
using VideoOS.Telemetry;

namespace VideoOS.Platform.Common.Telemetry;

public class MIPTelemetryEnabler : ITelemetryEnabler
{
	private readonly MIPTelemetryEnablerSettings _settings;

	private readonly IPrivacySettingWrapper _privacySettingsWrapper;

	private bool _isTelemetryEnabled;

	private DateTime _lastCheckOfAPITelemetry = DateTime.MinValue;

	private int _callsFailedConsecutively;

	public bool IsTelemetryEnabled
	{
		get
		{
			if (_lastCheckOfAPITelemetry.Add(_settings.RetryDelay) < DateTime.Now && _callsFailedConsecutively <= _settings.MaxConsecutiveErrors)
			{
				_lastCheckOfAPITelemetry = DateTime.Now;
				_isTelemetryEnabled = LookUpCollectTelemetry();
			}
			return _isTelemetryEnabled;
		}
	}

	public MIPTelemetryEnabler()
	{
		_settings = new MIPTelemetryEnablerSettings();
		_privacySettingsWrapper = new PrivacySettingWrapper();
	}

	public MIPTelemetryEnabler(MIPTelemetryEnablerSettings settings, IPrivacySettingWrapper privacySettingWrapper)
	{
		_settings = settings;
		_privacySettingsWrapper = privacySettingWrapper;
	}

	private bool LookUpCollectTelemetry()
	{
		try
		{
			bool collectTelemetryEnabled = _privacySettingsWrapper.GetCollectTelemetryEnabled();
			_callsFailedConsecutively = 0;
			return collectTelemetryEnabled;
		}
		catch
		{
			_callsFailedConsecutively++;
			if (_callsFailedConsecutively == _settings.MaxConsecutiveErrors)
			{
				PrepareForRetryAfterDelay();
			}
		}
		return false;
	}

	private void PrepareForRetryAfterDelay()
	{
		_lastCheckOfAPITelemetry = DateTime.Now.Subtract(_settings.RetryDelay).Add(_settings.RetryOnErrorDelay);
	}
}
