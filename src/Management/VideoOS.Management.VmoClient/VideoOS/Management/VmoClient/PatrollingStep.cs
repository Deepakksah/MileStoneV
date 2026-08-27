using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("Position {Position}, Preset {Preset.Id}")]
public sealed class PatrollingStep
{
	public static TimeSpan DefaultWaitTime = TimeSpan.FromSeconds(5.0);

	public static TimeSpan DefaultExpectedTransitionTime = TimeSpan.FromSeconds(3.0);

	public static double DefaultPanSpeed = 1.0;

	public static double DefaultTiltSpeed = 1.0;

	public static double DefaultZoomSpeed = 1.0;

	private int _position;

	private PatrollingPreset _preset;

	private TimeSpan _waitTime;

	private TimeSpan _expectedTransitionTimeToPreset;

	private double _panSpeed;

	private double _tiltSpeed;

	private double _zoomSpeed;

	public PatrollingProfile Profile { get; private set; }

	public int Position => _position;

	public PatrollingPreset Preset
	{
		get
		{
			return _preset;
		}
		set
		{
			_preset = value;
			Profile.WriteDefinition();
		}
	}

	public TimeSpan WaitTime
	{
		get
		{
			return _waitTime;
		}
		set
		{
			_waitTime = value;
			Profile.WriteDefinition();
		}
	}

	public TimeSpan ExpectedTransitionTimeToPreset
	{
		get
		{
			return _expectedTransitionTimeToPreset;
		}
		set
		{
			_expectedTransitionTimeToPreset = value;
			Profile.WriteDefinition();
		}
	}

	public double PanSpeed
	{
		get
		{
			return _panSpeed;
		}
		set
		{
			_panSpeed = value;
			Profile.WriteDefinition();
		}
	}

	public double TiltSpeed
	{
		get
		{
			return _tiltSpeed;
		}
		set
		{
			_tiltSpeed = value;
			Profile.WriteDefinition();
		}
	}

	public double ZoomSpeed
	{
		get
		{
			return _zoomSpeed;
		}
		set
		{
			_zoomSpeed = value;
			Profile.WriteDefinition();
		}
	}

	public PatrollingStep(PatrollingProfile patrollingProfile, PatrollingPreset preset)
		: this(patrollingProfile, preset, DefaultWaitTime)
	{
	}

	public PatrollingStep(PatrollingProfile patrollingProfile, PatrollingPreset preset, TimeSpan waitTime)
		: this(patrollingProfile, preset, waitTime, DefaultExpectedTransitionTime)
	{
	}

	public PatrollingStep(PatrollingProfile patrollingProfile, PatrollingPreset preset, TimeSpan waitTime, TimeSpan expectedTransitionTimeToPreset)
		: this(patrollingProfile, preset, waitTime, expectedTransitionTimeToPreset, DefaultPanSpeed, DefaultTiltSpeed, DefaultZoomSpeed)
	{
	}

	public PatrollingStep(PatrollingProfile patrollingProfile, PatrollingPreset preset, TimeSpan waitTime, TimeSpan expectedTransitionTimeToPreset, double panSpeed, double tiltSpeed, double zoomSpeed)
	{
		if (patrollingProfile == null)
		{
			throw new ArgumentNullException("patrollingProfile");
		}
		if (preset == null)
		{
			throw new ArgumentNullException("preset");
		}
		_position = -1;
		Profile = patrollingProfile;
		_preset = preset;
		_waitTime = waitTime;
		_panSpeed = panSpeed;
		_tiltSpeed = tiltSpeed;
		_zoomSpeed = zoomSpeed;
		_expectedTransitionTimeToPreset = expectedTransitionTimeToPreset;
	}

	public void SetPosition(int position)
	{
		_position = position;
	}
}
