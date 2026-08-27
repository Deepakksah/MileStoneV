using System;
using System.Windows.Forms;
using System.Windows.Threading;
using VideoOS.Platform.Data.Speaker;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class OutgoingSpeakerController : DirectSpeakerController
{
	public delegate void OutgoingAudioTransmitLockEventHandler(object sender, OutgoingAudioTransmitLockEventArgs e);

	public delegate void OutgoingAudioMeterEventHandler(object sender, OutgoingAudioMeterEventArgs e);

	private delegate void HandleAudioDataReadyEventDelegate(byte[] audioData);

	public class OutgoingAudioMeterEventArgs : EventArgs
	{
		protected int _outgoingAudioMeter;

		public virtual int OutgoingAudioMeter
		{
			get
			{
				return _outgoingAudioMeter;
			}
			set
			{
				_outgoingAudioMeter = value;
			}
		}

		public OutgoingAudioMeterEventArgs(int outgoingAudioMeter)
		{
			_outgoingAudioMeter = outgoingAudioMeter;
		}
	}

	private bool _outgoingAudioTransmitLock;

	private int _outgoingAudioMeter;

	private bool _outgoingAudioSupported;

	private IAudioRecorder _audioRecorder;

	[Obsolete("This property is no longer supported please use UIDispatcher instead.", false)]
	public Control UIHandle;

	public bool OutgoingAudioSupported
	{
		get
		{
			return _outgoingAudioSupported;
		}
		set
		{
			if (_outgoingAudioSupported != value)
			{
				_outgoingAudioSupported = value;
			}
		}
	}

	public Dispatcher UIDispatcher { get; set; }

	public bool OutgoingAudioTransmitLock
	{
		get
		{
			return _outgoingAudioTransmitLock;
		}
		set
		{
			if (_outgoingAudioTransmitLock != value)
			{
				_outgoingAudioTransmitLock = value;
				FireOutgoingAudioTransmitLockEvent(_outgoingAudioTransmitLock);
			}
		}
	}

	public int OutgoingAudioMeter
	{
		get
		{
			return _outgoingAudioMeter;
		}
		set
		{
			if (_outgoingAudioMeter != value)
			{
				_outgoingAudioMeter = value;
				FireOutgoingAudioMeterEvent(_outgoingAudioMeter);
			}
		}
	}

	public event OutgoingAudioTransmitLockEventHandler OutgoingAudioTransmitLockEvent;

	public event OutgoingAudioMeterEventHandler OutgoingAudioMeterEvent;

	public OutgoingSpeakerController()
	{
		new TrackPageViewData(typeof(OutgoingSpeakerController)).Send();
	}

	public void Init(IAudioRecorder audiorecorder)
	{
		if (base.Busy)
		{
			throw new InvalidOperationException("The controller is busy. Disconnect and/or try again later");
		}
		if (_audioRecorder != null)
		{
			_audioRecorder.Free();
		}
		_audioRecorder = audiorecorder;
		if (_audioRecorder.Initialize())
		{
			_audioRecorder.AudioDataReadyEvent += AudioRecorder_AudioDataReadyEvent;
			OutgoingAudioSupported = true;
		}
	}

	public override void Close(int timeoutMSec = 100)
	{
		StopCapturing();
		base.Close(timeoutMSec);
	}

	public override void SendData(byte[] audioData)
	{
		throw new NotSupportedException("Data is sent using audio recorder. To send data directly, use DirectSpeakerController");
	}

	private void AudioRecorder_AudioDataReadyEvent(object sender, IAudioRecorder.AudioDataEventArgs e)
	{
		try
		{
			if (UIDispatcher != null)
			{
				UIDispatcher.BeginInvoke(new HandleAudioDataReadyEventDelegate(HandleAudioDataReady), e.AudioData);
			}
			else if (UIHandle != null)
			{
				UIHandle.BeginInvoke(new HandleAudioDataReadyEventDelegate(HandleAudioDataReady), e.AudioData);
			}
			else
			{
				HandleAudioDataReady(e.AudioData);
			}
		}
		catch (Exception)
		{
		}
	}

	protected virtual void FireOutgoingAudioTransmitLockEvent(bool locked)
	{
		if (this.OutgoingAudioTransmitLockEvent != null)
		{
			this.OutgoingAudioTransmitLockEvent(this, new OutgoingAudioTransmitLockEventArgs(locked));
		}
	}

	protected virtual void FireOutgoingAudioMeterEvent(int outgoingAudioMeter)
	{
		if (this.OutgoingAudioMeterEvent != null)
		{
			this.OutgoingAudioMeterEvent(this, new OutgoingAudioMeterEventArgs(outgoingAudioMeter));
		}
	}

	private void HandleAudioDataReady(byte[] audioData)
	{
		if (audioData != null)
		{
			SendDataToImageServers(audioData, wrapData: true);
			int outgoingAudioMeter = _audioRecorder.CalcMeterValue(audioData, audioData.Length);
			FireOutgoingAudioMeterEvent(outgoingAudioMeter);
		}
	}

	public sealed override void ISClient_ClientStateEvent(object sender, ImageServerClient.ClientStateEventArgs e)
	{
		try
		{
			if (UIDispatcher != null)
			{
				UIDispatcher.BeginInvoke(new HandleClientStateEventDelegate(base.HandleClientState), e.ClientState, e.ErrorMessage, e.FailedDevices, e.ErrorCode);
			}
			else if (UIHandle != null)
			{
				UIHandle.BeginInvoke(new HandleClientStateEventDelegate(base.HandleClientState), e.ClientState, e.ErrorMessage, e.FailedDevices, e.ErrorCode);
			}
			else
			{
				HandleClientState(e.ClientState, e.ErrorMessage, e.FailedDevices, e.ErrorCode);
			}
		}
		catch (Exception)
		{
		}
	}

	protected sealed override void SetOutgoingSpeaker()
	{
		base.SetOutgoingSpeaker();
		if (OutgoingAudioTransmitLock)
		{
			OutgoingAudioTransmitLock = false;
		}
	}

	protected sealed override bool ConnectClients(int timeoutMsec = 10000)
	{
		ImageServerClient isc = new ImageServerClient(_audioRecorder.OutgoingWaveFormatChannels, _audioRecorder.OutgoingWaveFormatBitsPerSample, _audioRecorder.OutgoingAudioWaveFormatSamplesPerSecond);
		return ConnectClient(isc, timeoutMsec);
	}

	protected override bool DisconnectClients(int timeoutMsec = 10000)
	{
		if (base.HasAnyISClients)
		{
			StopCapturing();
		}
		return base.DisconnectClients(timeoutMsec);
	}

	protected sealed override bool StartTransmission(out bool starting)
	{
		bool result = base.StartTransmission(out starting);
		if (base.HasAnyISClients && !StartCapturing())
		{
			result = false;
		}
		return result;
	}

	protected bool StartCapturing()
	{
		bool result = true;
		try
		{
			if (_audioRecorder != null && !_audioRecorder.IsRecording())
			{
				_audioRecorder.StartRecording();
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "OutgoingAudioPlugin", "Start Capturing error: " + ex.Message);
			ShowErrorUsingLocalPCMicrophoneMessage("During recording start");
			result = false;
		}
		return result;
	}

	protected sealed override bool StopTransmission(out bool stopping)
	{
		bool num = StopCapturing();
		bool flag = base.StopTransmission(out stopping);
		return num && flag;
	}

	protected bool StopCapturing()
	{
		bool result = true;
		try
		{
			if (_audioRecorder != null)
			{
				_audioRecorder.StopRecording();
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "OutgoingAudioPlugin", "Stop Capturing error: " + ex.Message);
			ShowErrorUsingLocalPCMicrophoneMessage("During recording stop");
			result = false;
		}
		finally
		{
			FireOutgoingAudioMeterEvent(0);
		}
		return result;
	}
}
