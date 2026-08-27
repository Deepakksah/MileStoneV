using System;
using System.Collections.Generic;
using VideoOS.Platform.Data.MediaProviderInternal;

namespace VideoOS.Platform.Data;

public class MediaProviderChannel
{
	public delegate void SessionOpeningDelegate(MediaProviderSession session);

	public delegate void SessionClosedDelegate(MediaProviderSession session);

	public delegate MediaData GetMediaDataDelegate(MediaProviderSession session);

	public delegate object HandlePTZCommandDelegate(string controlCommandMessageId, object controlCommandData);

	private readonly int _channel = 1;

	private bool _toBeDisconnected;

	private bool _connected;

	private readonly List<RequestHeader> _activeSessions = new List<RequestHeader>();

	public int Channel => _channel;

	public Dictionary<string, string> ParameterDictionary { get; public set; }

	public Dictionary<string, string> SetParameterDictionary { get; public set; }

	public bool IsConnected
	{
		get
		{
			return _connected;
		}
		set
		{
		}
	}

	public bool InIdleMode { get; set; }

	public bool ToBeDisconnnected => _toBeDisconnected;

	public int ActiveSessions => _activeSessions.Count;

	public event SessionOpeningDelegate SessionOpening;

	public event SessionClosedDelegate SessionClosed;

	public event GetMediaDataDelegate GetMediaData;

	public event HandlePTZCommandDelegate HandlePTZCommandEvent;

	public MediaProviderChannel(int channel)
	{
		_channel = channel;
		ParameterDictionary = new Dictionary<string, string>();
		SetParameterDictionary = new Dictionary<string, string>();
		InIdleMode = true;
	}

	public void Init()
	{
	}

	public void Close()
	{
	}

	public bool Connect()
	{
		return false;
	}

	public void Disconnect()
	{
		_toBeDisconnected = true;
	}

	public void FireSessionOpeningEvent(RequestHeader requestHeader)
	{
		lock (_activeSessions)
		{
			_activeSessions.Add(requestHeader);
		}
		_connected = true;
		this.SessionOpening?.Invoke(requestHeader.MediaProviderSession);
	}

	public void FireSessionClosedEvent(RequestHeader requestHeader)
	{
		lock (_activeSessions)
		{
			if (_activeSessions.Contains(requestHeader))
			{
				_activeSessions.Remove(requestHeader);
			}
			if (_activeSessions.Count == 0)
			{
				_connected = false;
			}
		}
		this.SessionClosed?.Invoke(requestHeader.MediaProviderSession);
	}

	public MediaData ExecGetMediaDataEvent(RequestHeader requestHeader)
	{
		if (InIdleMode)
		{
			return GetMediaDataInIdle();
		}
		if (this.GetMediaData != null)
		{
			return this.GetMediaData(requestHeader.MediaProviderSession);
		}
		return null;
	}

	protected virtual MediaData GetMediaDataInIdle()
	{
		return new MediaData
		{
			Bytes = Array.Empty<byte>(),
			DateTime = DateTime.UtcNow,
			MediaContent = MediaContent.Jpeg
		};
	}

	public object FireHandlePTZCommandEvent(string controlCommandMessageId, object controlCommandData)
	{
		if (this.HandlePTZCommandEvent != null)
		{
			return this.HandlePTZCommandEvent(controlCommandMessageId, controlCommandData);
		}
		return null;
	}
}
