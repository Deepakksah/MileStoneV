using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Multicast;

public class MulticastRecorder
{
	public class SetDeviceInfoResult
	{
		public bool InUse;

		public Guid ExistingSessionId = Guid.Empty;
	}

	private Dictionary<Guid, MulticastDevice> _multicastDeviceCollection = new Dictionary<Guid, MulticastDevice>();

	private Dictionary<Guid, IRecorder> _allRecorderInstances = new Dictionary<Guid, IRecorder>();

	private Dictionary<Guid, List<Guid>> _allRecorderDevices = new Dictionary<Guid, List<Guid>>();

	private Guid _guid;

	private object _lockObject = new object();

	public Guid Guid => _guid;

	public IRecorder Recorder => _allRecorderInstances.Values.FirstOrDefault();

	public bool IsEmpty => _allRecorderInstances.Count == 0;

	public MulticastRecorder(Guid recorderId)
	{
		_guid = recorderId;
	}

	public void RegisterDevice(IRecorder recorder, Guid deviceId, out string ipAddress, out int port, out int ssrc, out string privacyMask, out int privacyMaskVersion)
	{
		lock (_lockObject)
		{
			if (!_multicastDeviceCollection.ContainsKey(deviceId))
			{
				MulticastDevice value = new MulticastDevice(deviceId);
				_multicastDeviceCollection[deviceId] = value;
			}
			if (!_allRecorderInstances.ContainsKey(recorder.Instance))
			{
				_allRecorderInstances[recorder.Instance] = recorder;
			}
			if (!_allRecorderDevices.ContainsKey(recorder.Instance))
			{
				_allRecorderDevices[recorder.Instance] = new List<Guid>();
			}
			if (!_allRecorderDevices[recorder.Instance].Contains(deviceId))
			{
				_allRecorderDevices[recorder.Instance].Add(deviceId);
			}
			_multicastDeviceCollection[deviceId].Register(out ipAddress, out port, out ssrc, out privacyMask, out privacyMaskVersion);
		}
	}

	public void UnregisterDevice(Guid deviceId)
	{
		lock (_lockObject)
		{
			if (_multicastDeviceCollection.TryGetValue(deviceId, out var value))
			{
				value.Unregister();
			}
		}
	}

	public MulticastDevice[] StartDevices()
	{
		ArrayList arrayList = new ArrayList();
		lock (_lockObject)
		{
			foreach (MulticastDevice value in _multicastDeviceCollection.Values)
			{
				if (value.Start())
				{
					arrayList.Add(value);
				}
			}
		}
		return (MulticastDevice[])arrayList.ToArray(typeof(MulticastDevice));
	}

	public MulticastDevice[] StopDevices(IRecorder recorder)
	{
		ArrayList arrayList = new ArrayList();
		lock (_lockObject)
		{
			foreach (MulticastDevice value2 in _multicastDeviceCollection.Values)
			{
				if (value2.Stop())
				{
					arrayList.Add(value2);
				}
			}
			foreach (MulticastDevice item in arrayList)
			{
				_multicastDeviceCollection.Remove(item.DeviceID);
				if (_allRecorderDevices.ContainsKey(recorder.Instance) && _allRecorderDevices[recorder.Instance].Contains(item.DeviceID))
				{
					_allRecorderDevices[recorder.Instance].Remove(item.DeviceID);
				}
			}
			if (_allRecorderDevices.TryGetValue(recorder.Instance, out var value) && !value.Any())
			{
				_allRecorderDevices.Remove(recorder.Instance);
				_allRecorderInstances.Remove(recorder.Instance);
			}
		}
		return (MulticastDevice[])arrayList.ToArray(typeof(MulticastDevice));
	}

	public MulticastDevice GetDevice(Guid deviceId)
	{
		if (_multicastDeviceCollection.TryGetValue(deviceId, out var value))
		{
			return value;
		}
		return null;
	}

	public Guid[] KeepAliveDevices(bool forceAll, out Guid[] sessionIds)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		lock (_lockObject)
		{
			foreach (MulticastDevice value in _multicastDeviceCollection.Values)
			{
				if (value.KeepAlive(forceAll))
				{
					arrayList.Add(value.DeviceID);
					arrayList2.Add(value.SessionID);
				}
			}
		}
		sessionIds = (Guid[])arrayList2.ToArray(typeof(Guid));
		return (Guid[])arrayList.ToArray(typeof(Guid));
	}

	public bool HasDevicesToRestart()
	{
		lock (_lockObject)
		{
			foreach (MulticastDevice value in _multicastDeviceCollection.Values)
			{
				if (value.InError())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasPendingDevices()
	{
		lock (_lockObject)
		{
			foreach (MulticastDevice value in _multicastDeviceCollection.Values)
			{
				if (value.HasPendingDevices())
				{
					return true;
				}
			}
		}
		return false;
	}

	public SetDeviceInfoResult SetDeviceInfo(Guid deviceId, Guid sessionId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion, TimeSpan keepAliveInterval, bool encrypted, TimeSpan keepAliveTimerInterval)
	{
		SetDeviceInfoResult setDeviceInfoResult = new SetDeviceInfoResult();
		lock (_lockObject)
		{
			if (_multicastDeviceCollection.TryGetValue(deviceId, out var value))
			{
				if (sessionId != value.SessionID)
				{
					setDeviceInfoResult.ExistingSessionId = value.SessionID;
				}
				string[] obj = new string[6] { "deviceId=", null, null, null, null, null };
				Guid guid = deviceId;
				obj[1] = guid.ToString();
				obj[2] = " existingSessionID=";
				obj[3] = value.SessionID.ToString();
				obj[4] = " newSessionID=";
				guid = sessionId;
				obj[5] = guid.ToString();
				MulticastLogger.LogDebug("MulticastRecorder.SetDeviceInfo", string.Concat(obj));
				value.SessionID = sessionId;
				value.IPAddress = ipAddress;
				value.Port = port;
				value.SSRC = ssrc;
				value.PrivacyMask = privacyMask;
				value.PrivacyMaskVersion = privacyMaskVersion;
				value.KeepAliveInterval = keepAliveInterval;
				value.Encrypted = encrypted;
				value.KeepAliveTimerInterval = keepAliveTimerInterval;
				if (value.NumUsers > 0)
				{
					setDeviceInfoResult.InUse = true;
				}
			}
		}
		return setDeviceInfoResult;
	}

	public void SetDeviceInError(Guid deviceId)
	{
		lock (_lockObject)
		{
			if (_multicastDeviceCollection.TryGetValue(deviceId, out var value))
			{
				value.SetError();
			}
		}
	}

	public void Clear()
	{
		lock (_lockObject)
		{
			_multicastDeviceCollection.Clear();
		}
	}

	public int Counter()
	{
		return _multicastDeviceCollection.Count;
	}
}
