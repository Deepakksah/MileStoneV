#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using VideoOS.Toolkit;

namespace VideoOS.Platform.Common.Audio;

public sealed class NAudioPlayerComponent : IAudioPlayerComponent, IDisposable
{
	public class CallbackHandler : IAudioToolkit.IAudioHandler, IAudioToolkit.IStateHandler
	{
		private readonly NAudioPlayerComponent _component;

		public CallbackHandler(NAudioPlayerComponent component)
		{
			_component = component;
		}

		public void HandleAudioData(IAudioToolkit.IAudioInformation audioInformation)
		{
			_component.HandleAudioData(this, audioInformation);
		}

		public void HandleStateChanged(IAudioToolkit.State state, IAudioToolkit.Flags flags)
		{
			_component.HandleStateChanged(this, state, flags);
		}
	}

	private class StateInfo
	{
		private DateTime StartTime { get; } = DateTime.UtcNow;

		public DateTime FirstPackageReceivedTime { get; set; } = DateTime.MinValue;

		public int NumberOfPackagesReceived { get; set; }

		public int NumberOfSkippedPackages { get; set; }

		public double TotalDurationReceived { get; set; }

		public double BufferedDurationBefore { get; set; } = -1.0;

		public double BufferedDurationAfter { get; set; } = -1.0;

		public DateTime LastPackageReceivedTime { get; set; } = DateTime.MinValue;

		public double LastPackageDuration { get; set; } = -1.0;

		public DateTime LastPackageTimeStamp { get; set; } = DateTime.MinValue;

		public bool LastPackageSkipped { get; set; }

		public PlaybackState PlaybackState { get; set; }

		public void Trace()
		{
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("-----------------------------------------------------");
			stringBuilder.AppendLine("StartTime:                " + StartTime.ToLocalTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("FirstPackageReceivedTime: " + FirstPackageReceivedTime.ToLocalTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("TimeSinceFirstPackage:    " + (LastPackageReceivedTime - FirstPackageReceivedTime).TotalMilliseconds);
			stringBuilder.AppendLine("NumberOfPackagesReceived: " + NumberOfPackagesReceived);
			stringBuilder.AppendLine("TotalDurationReceived:    " + TotalDurationReceived);
			stringBuilder.AppendLine("NumberOfSkippedPackages:  " + NumberOfSkippedPackages);
			stringBuilder.AppendLine("LastPackageReceivedTime:  " + LastPackageReceivedTime.ToLocalTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("LastPackageTimeStamp:     " + LastPackageTimeStamp.ToLocalTime().ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("LastPackageDuration:      " + LastPackageDuration);
			stringBuilder.AppendLine("LastPackageSkipped:       " + LastPackageSkipped);
			stringBuilder.AppendLine("BufferedDurationBefore:   " + BufferedDurationBefore.ToString("0000", CultureInfo.InvariantCulture) + ", " + (BufferedDurationBefore - 200.0).ToString("+0000;-0000;00000", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("BufferedDurationAfter:    " + BufferedDurationAfter.ToString("0000", CultureInfo.InvariantCulture) + ", " + (BufferedDurationAfter - 200.0).ToString("+0000;-0000;00000", CultureInfo.InvariantCulture));
			stringBuilder.AppendLine("PlaybackState:            " + ((object)PlaybackState/*cast due to constrained. prefix*/).ToString());
			stringBuilder.AppendLine("TimeDelta:                " + ((LastPackageReceivedTime - FirstPackageReceivedTime).TotalMilliseconds - TotalDurationReceived));
			stringBuilder.AppendLine("-----------------------------------------------------");
			System.Diagnostics.Trace.WriteLine(stringBuilder.ToString());
		}
	}

	private bool _connected;

	private bool _mute;

	private IAudioToolkit _audioToolkit;

	private IToolkitFactory _toolkitFactory;

	private BufferedWaveProvider _waveProvider;

	private double _volume = 1.0;

	private VolumeSampleProvider _muteProvider;

	private VolumeSampleProvider _volumeProvider;

	private readonly WaveOutEvent _waveOutEvent;

	private readonly object _audioLock = new object();

	private MmResult _lastResult;

	private bool _disposed;

	private StateInfo _stateInfo = new StateInfo();

	private readonly int _bufferedDurationThreshold;

	private CallbackHandler _currentHandler;

	private volatile bool _disconnectInProgress;

	private string _lastToken;

	private readonly object _configurationUpdateLock = new object();

	private bool _hasPreviouslyIgnoredAudioPacket;

	private const double _audioMeterAmplifyingFactor = 1.333;

	public bool SupportsPlayback => WaveOut.DeviceCount > 0;

	public bool Connected
	{
		get
		{
			return _connected;
		}
		private set
		{
			if (!object.Equals(_connected, value))
			{
				_connected = value;
				if (!_connected)
				{
					this.OnConnectionLost?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}

	public bool Mute
	{
		get
		{
			return _mute;
		}
		set
		{
			if (!object.Equals(_mute, value))
			{
				_mute = value;
				if (_muteProvider != null)
				{
					_muteProvider.Volume = ((!_mute) ? 1 : 0);
				}
			}
		}
	}

	public bool ReportAudioMeterValue { get; set; }

	public bool PlayingPlayback { get; private set; }

	public bool PlayingLive { get; private set; }

	public event EventHandler<EventArgs<bool>> OnConnectionAttemptCompleted = delegate
	{
	};

	public event EventHandler OnConnectionLost = delegate
	{
	};

	public event EventHandler<EventArgs<int>> OnAudioMeterValueChanged;

	public NAudioPlayerComponent(int desiredLatency, int bufferedDurationThreshold)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		_bufferedDurationThreshold = bufferedDurationThreshold;
		_waveOutEvent = new WaveOutEvent
		{
			DesiredLatency = desiredLatency
		};
	}

	~NAudioPlayerComponent()
	{
		Dispose(disposing: true);
	}

	public Task ScheduleDeletionOfToolkitFactory(IToolkitFactory toolkitFactory, IAudioToolkit toolkit)
	{
		if (toolkitFactory == null)
		{
			return Task.CompletedTask;
		}
		return Task.Run(delegate
		{
			lock (toolkitFactory)
			{
				if (toolkit != null)
				{
					toolkitFactory.DeleteInstance(toolkit);
				}
				toolkitFactory.Dispose();
			}
		}).ContinueWith(delegate
		{
		});
	}

	public void HandleAudioData(CallbackHandler handler, IAudioToolkit.IAudioInformation audioInformation)
	{
		//IL_034a: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Invalid comparison between Unknown and I4
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Invalid comparison between Unknown and I4
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		if (!IsCurrentHandler(handler))
		{
			return;
		}
		if (!PlayingLive && !PlayingPlayback)
		{
			_waveOutEvent.Stop();
			_ = _hasPreviouslyIgnoredAudioPacket;
			_hasPreviouslyIgnoredAudioPacket = true;
			return;
		}
		_hasPreviouslyIgnoredAudioPacket = false;
		int value = 0;
		lock (_audioLock)
		{
			try
			{
				int num = Convert.ToInt32(audioInformation.DataSize());
				byte[] array = new byte[num];
				Marshal.Copy(audioInformation.RawData(), array, 0, num);
				int num2 = Convert.ToInt32(audioInformation.SamplingRate());
				int num3 = Convert.ToInt32(audioInformation.NumberOfChannels());
				int num4 = Convert.ToInt32(audioInformation.BitDepth());
				double num5 = (double)Convert.ToInt32(audioInformation.DataSize()) / ((double)num3 * (double)num4 / 8.0) / (double)num2 * 1000.0;
				if (_stateInfo.FirstPackageReceivedTime == DateTime.MinValue)
				{
					_stateInfo.FirstPackageReceivedTime = DateTime.UtcNow;
				}
				_stateInfo.NumberOfPackagesReceived++;
				_stateInfo.TotalDurationReceived += num5;
				if (_waveProvider == null)
				{
					_waveProvider = new BufferedWaveProvider(new WaveFormat(num2, num4, num3))
					{
						BufferDuration = TimeSpan.FromSeconds(2.0),
						ReadFully = false,
						DiscardOnBufferOverflow = true
					};
					_volumeProvider = new VolumeSampleProvider(WaveExtensionMethods.ToSampleProvider((IWaveProvider)(object)_waveProvider));
					_volumeProvider.Volume = (float)_volume;
					_muteProvider = new VolumeSampleProvider((ISampleProvider)(object)_volumeProvider);
					_muteProvider.Volume = ((!Mute) ? 1 : 0);
					WaveExtensionMethods.Init((IWavePlayer)(object)_waveOutEvent, (ISampleProvider)(object)_muteProvider, false);
				}
				_stateInfo.LastPackageDuration = num5;
				_stateInfo.LastPackageReceivedTime = DateTime.UtcNow;
				_stateInfo.LastPackageTimeStamp = audioInformation.TimeStamp();
				_stateInfo.BufferedDurationBefore = _waveProvider.BufferedDuration.TotalMilliseconds;
				if ((int)_waveOutEvent.PlaybackState == 1 && _waveProvider.BufferedDuration.TotalMilliseconds == 0.0)
				{
					_waveOutEvent.Pause();
				}
				if ((int)_waveOutEvent.PlaybackState != 1 || _waveProvider.BufferedDuration.TotalMilliseconds < (double)_bufferedDurationThreshold)
				{
					_waveProvider.AddSamples(array, 0, num);
					_stateInfo.LastPackageSkipped = false;
				}
				else
				{
					_stateInfo.LastPackageSkipped = true;
					_stateInfo.NumberOfSkippedPackages++;
				}
				if ((int)_waveOutEvent.PlaybackState != 1 && _waveProvider.BufferedDuration.TotalMilliseconds > (double)(_bufferedDurationThreshold + 100))
				{
					_waveOutEvent.Play();
				}
				_stateInfo.PlaybackState = _waveOutEvent.PlaybackState;
				_stateInfo.BufferedDurationAfter = _waveProvider.BufferedDuration.TotalMilliseconds;
				if (ShouldCalculateAudioMeterValue())
				{
					value = CalculateAudioMeterValue(array, num, num4);
				}
				if ((int)_lastResult != 0)
				{
					Log(error: false, "Resuming audio data transfer.");
				}
				_lastResult = (MmResult)0;
			}
			catch (MmException ex)
			{
				MmException ex2 = ex;
				if (_lastResult != ex2.Result)
				{
					_lastResult = ex2.Result;
					Log(error: true, string.Format(CultureInfo.InvariantCulture, "Exception caught in NAudioPlayerComponent. Result: {0} Message: {1}", ex2.Result, ((Exception)(object)ex2).Message));
				}
			}
		}
		if (ShouldCalculateAudioMeterValue())
		{
			this.OnAudioMeterValueChanged?.Invoke(this, new EventArgs<int>(value));
		}
	}

	public void HandleStateChanged(CallbackHandler handler, IAudioToolkit.State state, IAudioToolkit.Flags flags)
	{
		if (IsCurrentHandler(handler))
		{
			switch (state)
			{
			case IAudioToolkit.State.Connected:
				Connected = true;
				OnConnected();
				break;
			case IAudioToolkit.State.ConnectionFailed:
				Connected = false;
				this.OnConnectionAttemptCompleted(this, new EventArgs<bool>(value: false));
				break;
			case IAudioToolkit.State.Disconnected:
			case IAudioToolkit.State.ConnectionLost:
				Connected = false;
				break;
			}
			_disconnectInProgress = false;
		}
	}

	private void OnConnected()
	{
		this.OnConnectionAttemptCompleted(this, new EventArgs<bool>(value: true));
		if (PlayingLive)
		{
			_audioToolkit?.DoLive();
			_stateInfo = new StateInfo();
		}
	}

	private bool IsCurrentHandler(CallbackHandler handler)
	{
		return object.Equals(_currentHandler, handler);
	}

	public void ConnectUsingConfiguration(string configuration)
	{
		if (string.IsNullOrWhiteSpace(configuration))
		{
			throw new ArgumentException("Invalid toolkit XML", "configuration");
		}
		lock (_configurationUpdateLock)
		{
			string token = GetToken(configuration);
			bool playingLive = PlayingLive;
			if (_toolkitFactory != null && !string.IsNullOrWhiteSpace(token) && token != _lastToken)
			{
				_audioToolkit.UpdateConfiguration("istoken", token);
			}
			else
			{
				if (_toolkitFactory != null)
				{
					_currentHandler = null;
					ScheduleDeletionOfToolkitFactory(_toolkitFactory, _audioToolkit);
					_audioToolkit = null;
					_toolkitFactory = null;
				}
				_connected = false;
				_toolkitFactory = new ToolkitFactory();
				_currentHandler = new CallbackHandler(this);
				_hasPreviouslyIgnoredAudioPacket = false;
				_audioToolkit = (IAudioToolkit)_toolkitFactory.CreateInstance(configuration);
				_audioToolkit.SetAudioHandler(_currentHandler);
				_audioToolkit.SetStateHandler(_currentHandler);
				_audioToolkit.Connect();
				if (playingLive)
				{
					PlayingLive = false;
					LiveStart();
				}
			}
			if (!string.IsNullOrWhiteSpace(token))
			{
				_lastToken = token;
			}
		}
	}

	public void SetConfiguration(string configuration)
	{
		ConnectUsingConfiguration(configuration);
	}

	public void Disconnect()
	{
		if (Connected)
		{
			_disconnectInProgress = true;
			_audioToolkit?.Disconnect();
		}
	}

	public void LiveStart()
	{
		if (!_disconnectInProgress && !PlayingLive)
		{
			PlayingLive = true;
			BufferedWaveProvider waveProvider = _waveProvider;
			if (waveProvider != null)
			{
				waveProvider.ClearBuffer();
			}
			if (!Connected)
			{
				_audioToolkit?.Connect();
			}
			_audioToolkit?.DoLive();
			_stateInfo = new StateInfo();
		}
	}

	public void LiveStop()
	{
		if (PlayingLive)
		{
			PlayingLive = false;
			_waveOutEvent.Stop();
			_audioToolkit?.StopLive();
			BufferedWaveProvider waveProvider = _waveProvider;
			if (waveProvider != null)
			{
				waveProvider.ClearBuffer();
			}
		}
	}

	public void PlaybackStart(DateTime startTime)
	{
		if (!_disconnectInProgress && !PlayingPlayback)
		{
			PlayingPlayback = true;
			BufferedWaveProvider waveProvider = _waveProvider;
			if (waveProvider != null)
			{
				waveProvider.ClearBuffer();
			}
			if (!Connected)
			{
				_audioToolkit?.Connect();
			}
			_audioToolkit?.DoPlayback(startTime, 1.0);
			_stateInfo = new StateInfo();
		}
	}

	public void PlaybackReSync(DateTime syncTime)
	{
		if (!_disconnectInProgress && Connected && PlayingPlayback)
		{
			_audioToolkit?.DoPlayback(syncTime, 1.0);
		}
	}

	public void PlaybackStop()
	{
		if (PlayingPlayback)
		{
			PlayingPlayback = false;
			_waveOutEvent.Stop();
			DateTime today = DateTime.Today;
			_audioToolkit?.DoPlayback(today, 0.0);
			BufferedWaveProvider waveProvider = _waveProvider;
			if (waveProvider != null)
			{
				waveProvider.ClearBuffer();
			}
		}
	}

	public void SetVolume(double volume)
	{
		Mute = false;
		_volume = volume;
		if (_volumeProvider != null)
		{
			_volumeProvider.Volume = (float)volume;
		}
	}

	private static int CalculateAudioMeterValue(byte[] data, int dataLength, int bitsPerSample)
	{
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		for (int i = 0; i < dataLength / bitsPerSample; i += 5)
		{
			num = 0.0;
			for (int j = 0; j < bitsPerSample / 8; j++)
			{
				num += (double)(data[i * bitsPerSample + j] << j * 8);
			}
			if (num > (double)(1 << bitsPerSample - 1))
			{
				num -= (double)(1 << bitsPerSample);
			}
			num2 += num * num;
			num3 += 1.0;
		}
		int val = (int)(Math.Sqrt(num2 / num3) / (double)(1 << bitsPerSample - 1) * 100.0 * 1.333);
		return Math.Min(100, Math.Max(0, val));
	}

	private bool ShouldCalculateAudioMeterValue()
	{
		if (!Mute)
		{
			return ReportAudioMeterValue;
		}
		return false;
	}

	private static void Log(bool error, string text)
	{
		EnvironmentManager.Instance.Log(error, "NAudioPlayerComponent", text);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			if (Connected)
			{
				Disconnect();
			}
			_disposed = true;
			_currentHandler = null;
			if (_toolkitFactory != null)
			{
				ScheduleDeletionOfToolkitFactory(_toolkitFactory, _audioToolkit).Wait();
				_audioToolkit = null;
				_toolkitFactory = null;
			}
			_waveOutEvent.Dispose();
		}
	}

	private static string GetToken(string configuration)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(configuration);
		}
		catch (XmlException)
		{
			return string.Empty;
		}
		XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("token");
		if (elementsByTagName.Count != 1)
		{
			return string.Empty;
		}
		return elementsByTagName[0].InnerText;
	}
}
