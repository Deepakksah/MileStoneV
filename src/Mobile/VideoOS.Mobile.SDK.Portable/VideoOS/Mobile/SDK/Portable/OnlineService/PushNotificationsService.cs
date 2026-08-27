using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.OnlineService;

public class PushNotificationsService : IDisposable
{
	private Connection _connection;

	private Guid _deviceId = Guid.Empty;

	private bool _enabled;

	private bool _disposed;

	private NotificationParamsHelper.CloudType _cloudType;

	public bool Enabled
	{
		set
		{
			_enabled = value;
		}
	}

	public PushNotificationsService(UriBuilder uri, Guid deviceId, string mobileClientVersion = null, NotificationParamsHelper.CloudType cloud = NotificationParamsHelper.CloudType.MS)
	{
		_deviceId = deviceId;
		_cloudType = cloud;
		Dictionary<string, string> headers = (string.IsNullOrWhiteSpace(mobileClientVersion) ? null : new Dictionary<string, string>
		{
			["MobileClientVersion"] = mobileClientVersion,
			["MobileServer-ProductCode"] = "1"
		});
		_connection = new Connection((!(uri.Scheme == "http")) ? ChannelTypes.HTTPSecure : ChannelTypes.HTTP, uri.Host, (uint)uri.Port, uri.Path.TrimStart(new char[1] { '/' }), plainAuthentication: true, requireCommandEncryption: false, null, headers)
		{
			RunHeartBeat = false,
			CommandsQueueing = CommandsQueueing.SendDirect
		};
	}

	public IAsyncResult RegisterPushNotifications(string deviceToken, Guid serverId, string[] optionalParams = null, OnResponse<RegisterForNotificationsResponse> onSuccess = null, OnResponse<RegisterForNotificationsResponse> onFail = null)
	{
		if (!_enabled)
		{
			return null;
		}
		NotificationParams notificationParams = new NotificationParams
		{
			Cloud = _cloudType,
			DeviceId = _deviceId,
			SettingsFlag = 1u
		};
		if (deviceToken != null)
		{
			notificationParams.DeviceToken = deviceToken;
		}
		if (serverId != Guid.Empty)
		{
			notificationParams.ServerId = serverId;
		}
		if (optionalParams != null)
		{
			switch (optionalParams.Count())
			{
			case 3:
				if (optionalParams[2] != null)
				{
					notificationParams.OptionalParamZ = optionalParams[2];
				}
				goto case 2;
			case 2:
				if (optionalParams[1] != null)
				{
					notificationParams.OptionalParamY = optionalParams[1];
				}
				goto case 1;
			case 1:
				if (optionalParams[0] != null)
				{
					notificationParams.OptionalParamX = optionalParams[0];
				}
				break;
			}
		}
		return _connection.Notifications.RegisterForNotifications(notificationParams, onSuccess, onFail);
	}

	public IAsyncResult UnregisterPushNotifications(string deviceToken, Guid serverId, OnResponse<UnregisterForNotificationsResponse> onSuccess = null, OnResponse<UnregisterForNotificationsResponse> onFail = null)
	{
		if (!_enabled)
		{
			return null;
		}
		NotificationParams notificationParams = new NotificationParams
		{
			Cloud = _cloudType,
			DeviceId = _deviceId,
			SettingsFlag = 0u
		};
		if (deviceToken != null)
		{
			notificationParams.DeviceToken = deviceToken;
		}
		if (serverId != Guid.Empty)
		{
			notificationParams.ServerId = serverId;
		}
		return _connection.Notifications.RegisterForNotifications(notificationParams, delegate(RegisterForNotificationsResponse responce)
		{
			onSuccess?.Invoke(new UnregisterForNotificationsResponse(responce));
		}, delegate(RegisterForNotificationsResponse responce)
		{
			onFail?.Invoke(new UnregisterForNotificationsResponse(responce));
		});
	}

	~PushNotificationsService()
	{
		Dispose();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_connection?.Stop();
				_connection?.Dispose();
				_connection = null;
			}
			_disposed = true;
		}
	}
}
