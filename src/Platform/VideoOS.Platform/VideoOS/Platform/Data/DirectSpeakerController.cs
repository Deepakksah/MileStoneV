using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using VideoOS.Platform.Data.Speaker;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class DirectSpeakerController : IDisposable, IDirectSpeakerController
{
	private enum ConnectState
	{
		UnInitialised,
		Disconnected,
		Connecting,
		Connected,
		Disconnecting
	}

	public delegate void HandleClientStateEventDelegate(VideoOS.Platform.Data.Speaker.ClientState clientState, string errorMessage, ArrayList failedDevices, int errorCode);

	public class ActiveOutgoingSpeakersChangedEventArgs : EventArgs
	{
		private FQID _activeOutgoingSpeaker;

		public virtual FQID ActiveOutgoingSpeaker
		{
			get
			{
				return _activeOutgoingSpeaker;
			}
			set
			{
				_activeOutgoingSpeaker = value;
			}
		}

		public ActiveOutgoingSpeakersChangedEventArgs(FQID activeOutgoingSpeaker)
		{
			_activeOutgoingSpeaker = activeOutgoingSpeaker;
		}
	}

	public class OutgoingAudioTransmitLockEventArgs : EventArgs
	{
		private bool _locked;

		public virtual bool Locked
		{
			get
			{
				return _locked;
			}
			set
			{
				_locked = value;
			}
		}

		public OutgoingAudioTransmitLockEventArgs(bool locked)
		{
			_locked = locked;
		}
	}

	private FQID _deviceFQID;

	private LoginSettings _loginSettings;

	private ConnectState _connectState;

	private bool _sendAudioToServer;

	private bool _outgoingAudioTransmitEnableState;

	private bool _mute;

	private object _lockObject = new object();

	private Dictionary<string, ImageServerClient> _isClients = new Dictionary<string, ImageServerClient>();

	private System.Timers.Timer _retryConnectTimer = new System.Timers.Timer();

	private int _retryConnectTimerInterval = 2;

	private string _outgoingAudioErrorText;

	private object _regObj1;

	public const int MaxAllowableTimeOutMSec = 10000;

	public const int DefAllowableTimeOutMSec = 100;

	private int _outgoingWaveFormatChannels;

	private int _outgoingWaveFormatBitsPerSample;

	private int _outgoingAudioWaveFormatSamplesPerSecond;

	public bool DoRetryConnect { get; set; }

	public LoginSettings LoginSettings
	{
		set
		{
			_loginSettings = value;
		}
	}

	protected bool HasAnyISClients => _isClients.Count > 0;

	public bool Connected => _connectState == ConnectState.Connected;

	public bool Busy
	{
		get
		{
			ConnectState connectState = _connectState;
			if ((uint)connectState <= 1u)
			{
				return false;
			}
			return true;
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
			if (_mute != value)
			{
				_mute = value;
				FireMuteStatusChangedEvent();
			}
		}
	}

	public bool OutgoingAudioTransmitEnableState
	{
		get
		{
			return _outgoingAudioTransmitEnableState;
		}
		set
		{
			if (_outgoingAudioTransmitEnableState != value)
			{
				_outgoingAudioTransmitEnableState = value;
				FireOutgoingAudioTransmitEnableStateChangedEvent();
			}
		}
	}

	public virtual string OutgoingAudioErrorText
	{
		get
		{
			return _outgoingAudioErrorText;
		}
		set
		{
			_outgoingAudioErrorText = value;
		}
	}

	public event EventHandler MuteStatusChangedEvent;

	public event EventHandler OutgoingAudioTransmitEnableStateChangedEvent;

	public event EventHandler OutgoingAudioErrorTextEvent;

	public DirectSpeakerController()
	{
		DoRetryConnect = false;
		_retryConnectTimer.Elapsed += RetryConnectTimerTick;
		_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
		new TrackPageViewData(typeof(DirectSpeakerController)).Send();
	}

	public virtual void Close(int timeoutMSec = 100)
	{
		if (timeoutMSec > 10000)
		{
			timeoutMSec = 10000;
		}
		else if (timeoutMSec < 100)
		{
			timeoutMSec = 100;
		}
		DisconnectClients(timeoutMSec);
		_sendAudioToServer = false;
		OutgoingAudioTransmitEnableState = false;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_regObj1 != null)
			{
				EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
				_regObj1 = null;
			}
			if (_retryConnectTimer != null)
			{
				_retryConnectTimer.Stop();
				_retryConnectTimer.Elapsed -= RetryConnectTimerTick;
				_retryConnectTimer.Dispose();
				_retryConnectTimer = null;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public virtual bool Connect(FQID speakerFQID, int timeoutMSec = 10000)
	{
		lock (_lockObject)
		{
			ConnectState connectState = _connectState;
			if ((uint)connectState > 1u)
			{
				return false;
			}
		}
		if (speakerFQID == null)
		{
			throw new ArgumentException("Speaker FQID cannot be null", "speakerFQID");
		}
		_deviceFQID = speakerFQID;
		if (_loginSettings == null)
		{
			_loginSettings = LoginSettingsCache.GetLoginSettings(_deviceFQID);
		}
		if (timeoutMSec > 10000)
		{
			timeoutMSec = 10000;
		}
		else if (timeoutMSec < 100)
		{
			timeoutMSec = 100;
		}
		OutgoingAudioTransmitEnableStateChangedEvent += AudioController_OutgoingAudioTransmitEnableStateChangedEvent;
		SetOutgoingSpeaker();
		return ConnectClients(timeoutMSec);
	}

	public void TransmitStart()
	{
		_sendAudioToServer = true;
		OutgoingAudioTransmissionStart();
	}

	public void TransmitStop()
	{
		_sendAudioToServer = false;
		OutgoingAudioTransmissionStop();
	}

	public virtual void SendData(byte[] audioData)
	{
		if (audioData != null)
		{
			SendDataToImageServers(audioData, wrapData: false);
		}
	}

	public void InitOutgoingWaveFormat(int channelsCount, int bitsPerSample, int samplesPerSecond)
	{
		_outgoingWaveFormatChannels = channelsCount;
		_outgoingWaveFormatBitsPerSample = bitsPerSample;
		_outgoingAudioWaveFormatSamplesPerSecond = samplesPerSecond;
	}

	public void SendWrappedData(byte[] audioData)
	{
		if (_outgoingWaveFormatChannels == 0 || _outgoingWaveFormatBitsPerSample == 0 || _outgoingAudioWaveFormatSamplesPerSecond == 0)
		{
			throw new ArgumentException("Wave format channels, bits per sample or samples per seccond is 0. Call InitOutgoingWaveFormat with non-zero values.");
		}
		if (audioData != null)
		{
			SendDataToImageServers(audioData, wrapData: true);
		}
	}

	private void AudioController_OutgoingAudioTransmitEnableStateChangedEvent(object sender, EventArgs e)
	{
		if (_sendAudioToServer)
		{
			OutgoingAudioTransmissionStart();
		}
		else
		{
			OutgoingAudioTransmissionStop();
		}
	}

	private object SystemConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (!(message.Data is List<FQID> source) || _deviceFQID == null)
		{
			return null;
		}
		using (IEnumerator<FQID> enumerator = source.Where((FQID fqid) => fqid.ObjectId == _deviceFQID.ObjectId && fqid.ServerId.UserContext == _deviceFQID.ServerId.UserContext).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				FQID current = enumerator.Current;
				Item item = ((current.ServerId.UserContext == null) ? Configuration.Instance.GetItem(current) : current.ServerId.UserContext.Configuration.GetItem(current));
				if (item != null && !item.FQID.Equals(_deviceFQID))
				{
					_deviceFQID = item.FQID;
					if (_connectState == ConnectState.Connected)
					{
						DisconnectClients(10);
						Connect(item.FQID);
					}
				}
			}
		}
		return null;
	}

	protected virtual void FireMuteStatusChangedEvent()
	{
		this.MuteStatusChangedEvent?.Invoke(this, new EventArgs());
	}

	protected virtual void FireOutgoingAudioTransmitEnableStateChangedEvent()
	{
		this.OutgoingAudioTransmitEnableStateChangedEvent?.Invoke(this, new EventArgs());
	}

	public virtual void ISClient_ClientStateEvent(object sender, ImageServerClient.ClientStateEventArgs e)
	{
		try
		{
			HandleClientState(e.ClientState, e.ErrorMessage, e.FailedDevices, e.ErrorCode);
		}
		catch (Exception)
		{
		}
	}

	private void RetryConnectTimerTick(object sender, ElapsedEventArgs e)
	{
		_retryConnectTimer.Stop();
		if (DoRetryConnect && !(_deviceFQID == null) && !Connected)
		{
			ConnectClients(0);
		}
	}

	protected virtual void FireOutgoingAudioErrorTextEvent()
	{
		this.OutgoingAudioErrorTextEvent?.Invoke(this, new EventArgs());
	}

	public void HandleClientState(VideoOS.Platform.Data.Speaker.ClientState clientState, string errorMessage, ArrayList failedDevices, int errorCode)
	{
		if (failedDevices != null)
		{
			foreach (Guid failedDevice in failedDevices)
			{
				EnvironmentManager.Instance.Log(error: false, "OutgoingAudioPlugin", "Failed Device: " + failedDevice.ToString());
			}
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		switch (_connectState)
		{
		case ConnectState.Connecting:
		case ConnectState.Connected:
			flag2 = true;
			foreach (ImageServerClient value in _isClients.Values)
			{
				switch (value.ClientState)
				{
				case VideoOS.Platform.Data.Speaker.ClientState.Initialized:
					flag3 = true;
					break;
				case VideoOS.Platform.Data.Speaker.ClientState.Connecting:
				case VideoOS.Platform.Data.Speaker.ClientState.Connected:
				case VideoOS.Platform.Data.Speaker.ClientState.Initializing:
					flag2 = false;
					break;
				case VideoOS.Platform.Data.Speaker.ClientState.BeginningSpeak:
				case VideoOS.Platform.Data.Speaker.ClientState.Speaking:
					flag3 = true;
					if (value.FailedDeviceIds.Count > 0)
					{
						flag = true;
					}
					break;
				case VideoOS.Platform.Data.Speaker.ClientState.Disconnected:
					errorMessage = "Internal error. Invalid state: Disconnected";
					flag = true;
					break;
				case VideoOS.Platform.Data.Speaker.ClientState.ErrorDetected:
					flag = true;
					break;
				}
			}
			if (flag2)
			{
				if (flag3)
				{
					OutgoingAudioErrorText = "";
					_retryConnectTimer.Stop();
					_retryConnectTimerInterval = 2;
					_connectState = ConnectState.Connected;
				}
				else
				{
					flag = true;
					if (DoRetryConnect)
					{
						RetryConnectLater();
					}
				}
			}
			OutgoingAudioTransmitEnableState = flag2 && flag3;
			break;
		case ConnectState.Disconnecting:
			flag4 = true;
			foreach (ImageServerClient value2 in _isClients.Values)
			{
				if (value2.ClientState != VideoOS.Platform.Data.Speaker.ClientState.Disconnected)
				{
					flag4 = false;
				}
			}
			if (flag4)
			{
				_connectState = ConnectState.Disconnected;
			}
			break;
		}
		if (flag)
		{
			ShowErrorInISClientCommunicationMessage(errorMessage, errorCode);
		}
	}

	private void OutgoingAudioTransmissionStart()
	{
		if (!StartTransmission(out var _))
		{
			ShowErrorInISClientCommunicationMessage("During transmission start", 1);
			if (DoRetryConnect)
			{
				RetryConnectLater();
			}
		}
	}

	private void OutgoingAudioTransmissionStop()
	{
		if (!StopTransmission(out var stopping))
		{
			ShowErrorInISClientCommunicationMessage("During transmission stop", 1);
		}
		if (stopping)
		{
			OutgoingAudioTransmitEnableState = false;
		}
	}

	protected virtual void SetOutgoingSpeaker()
	{
		OutgoingAudioErrorText = "";
		if (_deviceFQID == null || _connectState != ConnectState.Connected)
		{
			OutgoingAudioTransmitEnableState = false;
		}
	}

	protected void RetryConnectLater()
	{
		DisconnectClients(0);
		OutgoingAudioTransmitEnableState = false;
		_retryConnectTimer.Interval = _retryConnectTimerInterval * 1000;
		_retryConnectTimer.Start();
		if (_retryConnectTimerInterval < 60)
		{
			_retryConnectTimerInterval += 2;
		}
	}

	protected virtual bool ConnectClients(int timeoutMsec = 10000)
	{
		ImageServerClient isc = new ImageServerClient(_outgoingWaveFormatChannels, _outgoingWaveFormatBitsPerSample, _outgoingAudioWaveFormatSamplesPerSecond);
		return ConnectClient(isc, timeoutMsec);
	}

	public bool ConnectClient(ImageServerClient isc, int timeoutMsec = 10000)
	{
		bool flag = true;
		lock (_lockObject)
		{
			switch (_connectState)
			{
			case ConnectState.Connecting:
			case ConnectState.Connected:
				return true;
			case ConnectState.Disconnecting:
				return false;
			default:
				_connectState = ConnectState.Connecting;
				break;
			}
		}
		isc.ClientStateEvent += ISClient_ClientStateEvent;
		isc.RecorderName = _deviceFQID.ServerId.ServerHostname;
		isc.ServerUri = _deviceFQID.ServerId.Uri;
		isc.Token = _loginSettings.Token;
		isc.DeviceIds = new List<Guid>();
		isc.DeviceIds.Add(_deviceFQID.ObjectId);
		_isClients[_deviceFQID.ServerId.ServerHostname] = isc;
		foreach (ImageServerClient value in _isClients.Values)
		{
			if (!value.Connect())
			{
				flag = false;
			}
		}
		if (!flag)
		{
			DisconnectClients(0);
		}
		DateTime now = DateTime.Now;
		do
		{
			lock (_lockObject)
			{
				switch (_connectState)
				{
				case ConnectState.Connected:
					return flag;
				case ConnectState.Disconnected:
					return false;
				case ConnectState.UnInitialised:
					return false;
				case ConnectState.Connecting:
				case ConnectState.Disconnecting:
					break;
				}
			}
			Thread.Sleep(200);
		}
		while ((DateTime.Now - now).TotalMilliseconds < (double)timeoutMsec);
		return false;
	}

	protected virtual bool DisconnectClients(int timeoutMsec = 10000)
	{
		bool flag = true;
		lock (_lockObject)
		{
			ConnectState connectState = _connectState;
			if ((uint)connectState <= 1u || connectState == ConnectState.Disconnecting)
			{
				return true;
			}
			_connectState = ConnectState.Disconnecting;
		}
		ArrayList arrayList = new ArrayList();
		foreach (ImageServerClient value in _isClients.Values)
		{
			if (value != null)
			{
				if (!value.Disconnect())
				{
					flag = false;
				}
				value.ClientStateEvent -= ISClient_ClientStateEvent;
				string recorderName = value.RecorderName;
				arrayList.Add(recorderName);
			}
		}
		if (timeoutMsec > 10000)
		{
			timeoutMsec = 10000;
		}
		DateTime now = DateTime.Now;
		flag = false;
		TimeSpan timeSpan;
		do
		{
			ConnectState connectState = _connectState;
			if ((uint)connectState <= 1u)
			{
				flag = true;
			}
			Thread.Sleep(200);
			timeSpan = DateTime.Now - now;
		}
		while (!flag && timeSpan.TotalMilliseconds < (double)timeoutMsec);
		foreach (string item in arrayList)
		{
			_isClients.Remove(item);
		}
		_connectState = ConnectState.Disconnected;
		return flag;
	}

	protected virtual bool StartTransmission(out bool starting)
	{
		bool result = true;
		starting = false;
		foreach (ImageServerClient value in _isClients.Values)
		{
			if (value != null && value.ClientState != VideoOS.Platform.Data.Speaker.ClientState.ErrorDetected && !value.IsSpeaking())
			{
				starting = true;
				if (!value.BeginSpeak())
				{
					result = false;
				}
			}
		}
		return result;
	}

	protected virtual bool StopTransmission(out bool stopping)
	{
		bool result = true;
		stopping = false;
		foreach (ImageServerClient value in _isClients.Values)
		{
			if (value != null && value.IsSpeaking())
			{
				stopping = true;
				if (!value.EndSpeak())
				{
					result = false;
				}
			}
		}
		return result;
	}

	protected void SendDataToImageServers(byte[] audioData, bool wrapData)
	{
		foreach (ImageServerClient value in _isClients.Values)
		{
			if (value != null && value.ClientState == VideoOS.Platform.Data.Speaker.ClientState.Speaking)
			{
				value.Speak(audioData, wrapData);
			}
		}
	}

	protected void ShowErrorUsingLocalPCMicrophoneMessage(string errorMessage)
	{
		OutgoingAudioErrorText = "OutgoingAudioErrorUsingLocalPCMicrophone";
		FireOutgoingAudioErrorTextEvent();
		EnvironmentManager.Instance.Log(error: false, "OutgoingAudioPlugin", "Audio Recorder error: " + errorMessage);
	}

	protected virtual void ShowErrorInISClientCommunicationMessage(string errorMessage, int errorCode)
	{
		OutgoingAudioErrorText = "OutgoingAudioErrorSendingAudioToSpeaker";
		FireOutgoingAudioErrorTextEvent();
		EnvironmentManager.Instance.Log(error: false, "OutgoingAudioPlugin", "ISClient error: " + errorMessage);
	}
}
