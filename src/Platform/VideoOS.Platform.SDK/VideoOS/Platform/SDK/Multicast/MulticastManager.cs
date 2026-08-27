using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Multicast;

public class MulticastManager
{
	public delegate void NoActiveRecordersEventHandler(MulticastManager multicastManager);

	public delegate void MulticastConfigEventHandler(object sender, MulticastConfigEventArgs e);

	public delegate void ResultCallbackEventHandler(object sender, AsyncResultEventArgs args);

	private int _multicastKeepAliveTimerInterval = 10000;

	private int _multicastRestartTimerInterval = 5000;

	private ClientCapabilities _clientCapabilities = new ClientCapabilities
	{
		PrivacyMask = true,
		PrivacyMaskVersion = 2
	};

	private object _lockObject = new object();

	private ServerId _serverId;

	private Dictionary<Guid, MulticastRecorder> _multicastRecorderCollection = new Dictionary<Guid, MulticastRecorder>();

	private Timer _multicastKeepAliveTimer = new Timer();

	private Timer _multicastRestartTimer = new Timer();

	public ServerId ServerId => _serverId;

	public event NoActiveRecordersEventHandler NoActiveRecordersEvent;

	public event MulticastConfigEventHandler MulticastConfigEvent = delegate
	{
	};

	public MulticastManager(ServerId serverId)
	{
		_serverId = serverId;
	}

	private void FireNoActiveRecordersEvent()
	{
		if (this.NoActiveRecordersEvent != null)
		{
			this.NoActiveRecordersEvent(this);
		}
	}

	private void FireMulticastConfigEvent(IRecorder recorder, Guid deviceId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion, Guid existingSessionId)
	{
		this.MulticastConfigEvent(this, new MulticastConfigEventArgs(recorder, deviceId, ipAddress, port, ssrc, privacyMask, privacyMaskVersion, existingSessionId));
	}

	private void MulticastKeepAliveTimerElapsed(object sender, ElapsedEventArgs e)
	{
		MulticastKeepAlive();
	}

	private void MulticastRestartTimerElapsed(object sender, ElapsedEventArgs e)
	{
		MulticastRestart();
	}

	public void Init()
	{
		_multicastKeepAliveTimer.Interval = _multicastKeepAliveTimerInterval;
		_multicastKeepAliveTimer.Elapsed += MulticastKeepAliveTimerElapsed;
		_multicastKeepAliveTimer.Enabled = true;
		_multicastKeepAliveTimer.Start();
		_multicastRestartTimer.Interval = _multicastRestartTimerInterval;
		_multicastRestartTimer.Elapsed += MulticastRestartTimerElapsed;
		_multicastRestartTimer.Enabled = true;
	}

	public void Close()
	{
		if (_multicastKeepAliveTimer != null)
		{
			_multicastKeepAliveTimer.Stop();
			_multicastKeepAliveTimer.Dispose();
			_multicastKeepAliveTimer = null;
		}
		if (_multicastRestartTimer != null)
		{
			_multicastRestartTimer.Stop();
			_multicastRestartTimer.Dispose();
			_multicastRestartTimer = null;
		}
	}

	public void RegisterDevice(IRecorder recorder, Guid deviceId, out string ipAddress, out int port, out int ssrc, out string privacyMask, out int privacyMaskVersion)
	{
		lock (_lockObject)
		{
			MulticastLogger.LogDebug("MulticastManager.RegisterDevice", "Multicast - Register device: " + deviceId.ToString());
			ipAddress = string.Empty;
			port = 0;
			ssrc = 0;
			privacyMask = string.Empty;
			privacyMaskVersion = 0;
			if (recorder != null)
			{
				if (!_multicastRecorderCollection.ContainsKey(recorder.Guid))
				{
					_multicastRecorderCollection[recorder.Guid] = new MulticastRecorder(recorder.Guid);
				}
				_multicastRecorderCollection[recorder.Guid].RegisterDevice(recorder, deviceId, out ipAddress, out port, out ssrc, out privacyMask, out privacyMaskVersion);
			}
		}
	}

	public void UnregisterDevice(IRecorder recorder, Guid deviceId)
	{
		lock (_lockObject)
		{
			MulticastLogger.LogDebug("MulticastManager.UnregisterDevice", "Multicast - Unregister device: " + deviceId.ToString());
			if (recorder != null && _multicastRecorderCollection.TryGetValue(recorder.Guid, out var value))
			{
				value.UnregisterDevice(deviceId);
			}
		}
	}

	private void SetError(IRecorder recorder, Guid deviceId)
	{
		lock (_lockObject)
		{
			if (recorder != null && _multicastRecorderCollection.TryGetValue(recorder.Guid, out var value))
			{
				value.SetDeviceInError(deviceId);
				_multicastRestartTimer.Start();
			}
		}
	}

	public MulticastDevice GetDevice(IRecorder recorder, Guid deviceId)
	{
		lock (_lockObject)
		{
			if (recorder != null && _multicastRecorderCollection.TryGetValue(recorder.Guid, out var value))
			{
				return value.GetDevice(deviceId);
			}
		}
		return null;
	}

	private void SetDeviceInfo(IRecorder recorder, Guid deviceId, Guid sessionId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion, TimeSpan keepAliveInterval, bool encrypted, bool success)
	{
		bool flag = false;
		Guid deviceId2 = Guid.Empty;
		Guid existingSessionId = Guid.Empty;
		int port2 = 0;
		int ssrc2 = 0;
		int privacyMaskVersion2 = 0;
		string ipAddress2 = string.Empty;
		string privacyMask2 = string.Empty;
		lock (_lockObject)
		{
			if (recorder != null && _multicastRecorderCollection.ContainsKey(recorder.Guid))
			{
				if (!encrypted && success)
				{
					TimeSpan keepAliveTimerInterval = new TimeSpan(0, 0, 0, 0, _multicastKeepAliveTimerInterval);
					MulticastRecorder.SetDeviceInfoResult setDeviceInfoResult = _multicastRecorderCollection[recorder.Guid].SetDeviceInfo(deviceId, sessionId, ipAddress, port, ssrc, privacyMask, privacyMaskVersion, keepAliveInterval, encrypted, keepAliveTimerInterval);
					if (setDeviceInfoResult.InUse)
					{
						MulticastConfig(recorder, deviceId, ipAddress, port, ssrc, privacyMask, privacyMaskVersion, setDeviceInfoResult.ExistingSessionId);
						deviceId2 = deviceId;
						ipAddress2 = ipAddress;
						port2 = port;
						ssrc2 = ssrc;
						privacyMask2 = privacyMask;
						privacyMaskVersion2 = privacyMaskVersion;
						existingSessionId = setDeviceInfoResult.ExistingSessionId;
					}
					else
					{
						MulticastConfig(recorder, Guid.Empty, string.Empty, 0, 0, string.Empty, 0, Guid.Empty);
					}
				}
				else
				{
					_multicastRecorderCollection[recorder.Guid].UnregisterDevice(deviceId);
					MulticastConfig(recorder, deviceId, string.Empty, 0, 0, string.Empty, 0, Guid.Empty);
					deviceId2 = deviceId;
				}
				flag = true;
			}
		}
		if (flag)
		{
			FireMulticastConfigEvent(recorder, deviceId2, ipAddress2, port2, ssrc2, privacyMask2, privacyMaskVersion2, existingSessionId);
		}
	}

	private MulticastDevice[] ProcessStart()
	{
		lock (_lockObject)
		{
			foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
			{
				MulticastDevice[] array = value.StartDevices();
				if (array.Length != 0)
				{
					MulticastLogger.LogDebug("MulticastManager.ProcessStart", "Multicast - Starting # devices: " + array.Length);
					return array;
				}
			}
			return null;
		}
	}

	private MulticastDevice[] ProcessStop(IRecorder recorder)
	{
		lock (_lockObject)
		{
			foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
			{
				MulticastDevice[] array = value.StopDevices(recorder);
				if (array.Length != 0)
				{
					MulticastLogger.LogDebug("MulticastManager.ProcessStop", "Multicast - Stopping # devices: " + array.Length);
					return array;
				}
			}
			return null;
		}
	}

	private int Counter()
	{
		int num = 0;
		lock (_lockObject)
		{
			foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
			{
				num += value.Counter();
			}
			return num;
		}
	}

	private void MulticastKeepAlive()
	{
		lock (_lockObject)
		{
			List<MulticastRecorder> list = new List<MulticastRecorder>();
			foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
			{
				Guid[] sessionIds;
				Guid[] array = value.KeepAliveDevices(forceAll: false, out sessionIds);
				if (array != null && array.Length != 0)
				{
					MulticastLogger.LogDebug("MulticastManager.MulticastKeepAlive", "# devices: " + array.Length);
					MulticastContinue(value.Recorder, array, sessionIds);
				}
				else if (value.IsEmpty)
				{
					list.Add(value);
				}
			}
			foreach (MulticastRecorder item in list)
			{
				_multicastRecorderCollection.Remove(item.Guid);
			}
			if (_multicastRecorderCollection.Count == 0)
			{
				FireNoActiveRecordersEvent();
			}
		}
	}

	private void MulticastRestart()
	{
		lock (_lockObject)
		{
			bool flag = false;
			foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
			{
				if (value.HasDevicesToRestart())
				{
					MulticastLogger.LogDebug("MulticastManager.MulticastRestart", "Multicast - Has devices to start");
					ProcessNewMulticastDevices(value.Recorder);
					flag = true;
				}
				else if (value.HasPendingDevices())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				_multicastRestartTimer.Stop();
			}
		}
	}

	public void ProcessNewMulticastDevices(IRecorder recorder)
	{
		for (MulticastDevice[] array = ProcessStart(); array != null; array = ProcessStart())
		{
			AsyncMulticastStart(array, recorder);
		}
	}

	private void AsyncMulticastStart(MulticastDevice[] multicastDevices, IRecorder recorder)
	{
		try
		{
			List<Guid> deviceIds = new List<Guid>();
			foreach (MulticastDevice multicastDevice in multicastDevices)
			{
				deviceIds.Add(multicastDevice.DeviceID);
			}
			Task.Run(async delegate
			{
				MulticastInfo[] array = await recorder.AsyncMulticastStart(deviceIds, _clientCapabilities);
				if (array != null)
				{
					MulticastInfo[] array2 = array;
					foreach (MulticastInfo multicastInfo in array2)
					{
						MulticastLogger.LogDebug("MulticastManager.HandleCommandServiceAsyncResult", "AsyncMulticastStart response. Id: " + multicastInfo.DeviceId.ToString() + ", SessionId: " + multicastInfo.MulticastSessionId.ToString() + " Address: " + multicastInfo.IPAddress.ToString() + ", Port: " + multicastInfo.ClientPort);
						TimeSpan keepAliveInterval = new TimeSpan(10 * multicastInfo.KeepAliveInterval.MicroSeconds);
						if (FindRecorderFromDeviceId(multicastInfo.DeviceId) == null)
						{
							MulticastLogger.LogError("MulticastManager", "Unable to locate multicast device at HandleCommandServiceAsyncResult");
						}
						else
						{
							SetDeviceInfo(recorder, multicastInfo.DeviceId, multicastInfo.MulticastSessionId, multicastInfo.IPAddress, multicastInfo.ClientPort, (int)multicastInfo.SSRC, multicastInfo.PrivacyMask, multicastInfo.PrivacyMaskVersion, keepAliveInterval, multicastInfo.Encrypted, multicastInfo.Success);
						}
					}
				}
				else if (deviceIds != null)
				{
					foreach (Guid item in deviceIds)
					{
						IRecorder recorder2 = FindRecorderFromDeviceId(item);
						if (recorder2 == null)
						{
							MulticastLogger.LogError("MulticastManager", "Unable to locate multicast device at HandleCommandServiceAsyncResult");
						}
						else
						{
							SetError(recorder2, item);
						}
					}
				}
			});
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("AsyncMulticastStart", ex.Message);
		}
	}

	public void ProcessDeletedMulticastDevices(IRecorder recorder)
	{
		for (MulticastDevice[] array = ProcessStop(recorder); array != null; array = ProcessStop(recorder))
		{
			AsyncMulticastStop(recorder, array);
		}
		MulticastLogger.LogDebug("MulticastManager", "Multicast - Number of devices left: " + Counter());
	}

	private static void AsyncMulticastStop(IRecorder recorder, MulticastDevice[] multicastDevices)
	{
		try
		{
			List<Guid> list = new List<Guid>();
			List<Guid> sessionIds = new List<Guid>();
			foreach (MulticastDevice multicastDevice in multicastDevices)
			{
				list.Add(multicastDevice.DeviceID);
				sessionIds.Add(multicastDevice.SessionID);
			}
			Task.Run(() => recorder.AsyncMulticastStop(sessionIds));
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("AsyncMulticastStop", ex.Message);
		}
	}

	private static void MulticastContinue(IRecorder recorder, Guid[] deviceIds, Guid[] sessionIds)
	{
		try
		{
			MulticastLogger.LogDebug("MulticastManagerFactory.MulticastContinue", "Multicast - AsyncMulticastContinue. No of devices: " + deviceIds.Length);
			AsyncMulticastContinue(recorder, deviceIds, sessionIds);
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("MulticastManagerFactory.MulticastContinue", ex.Message);
		}
	}

	private void MulticastConfig(IRecorder recorder, Guid deviceId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion, Guid existingSessionId)
	{
		try
		{
			if (ipAddress == string.Empty)
			{
				ProcessDeletedMulticastDevices(recorder);
			}
			if (deviceId != Guid.Empty && existingSessionId != Guid.Empty)
			{
				ProcessObsoleteMulticastDeviceSession(deviceId, existingSessionId);
			}
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("MulticastConfig", ex.Message);
		}
	}

	private static void AsyncMulticastContinue(IRecorder recorder, Guid[] deviceIds, Guid[] sessionIds)
	{
		try
		{
			Task.Run(() => recorder.AsyncMulticastContinue(sessionIds));
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("AsyncMulticastContinue", ex.Message);
		}
	}

	private void ProcessObsoleteMulticastDeviceSession(Guid deviceId, Guid obsoleteSessionId)
	{
		try
		{
			IRecorder recorder = FindRecorderFromDeviceId(deviceId);
			if (recorder == null)
			{
				MulticastLogger.LogError("ProcessObsoleteMulticastDeviceSession", "Unable to locate Recorder from DeviceId");
				return;
			}
			MulticastDevice multicastDevice = GetDevice(recorder, deviceId);
			if (multicastDevice != null)
			{
				Task.Run(() => recorder.AsyncMulticastStop(new List<Guid> { multicastDevice.DeviceID }));
			}
		}
		catch (Exception ex)
		{
			MulticastLogger.LogError("MulticastManager", ex.Message);
		}
	}

	private IRecorder FindRecorderFromDeviceId(Guid deviceId)
	{
		foreach (MulticastRecorder value in _multicastRecorderCollection.Values)
		{
			if (value.GetDevice(deviceId) != null)
			{
				return value.Recorder;
			}
		}
		return null;
	}
}
