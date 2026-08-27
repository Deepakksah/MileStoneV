using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.ServiceChannel;

public sealed class ServiceChannelManager : IServiceChannelManager
{
	private ILogger logger;

	private int serverPort;

	private static SCHMessageCreatorManagerClient client;

	public ServiceChannelManager(ILogger logger, int port)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		this.logger = logger;
		serverPort = port;
	}

	public Task SendHardwareMovedMessageAsync(Guid hardwareId, Guid sourceRecordingServerId, Guid destinationRecordingServerId)
	{
		string messageTypeName = "HardwareMoved";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["FromRecordingServerId"] = sourceRecordingServerId.ToString();
		dictionary["ToRecordingServerId"] = destinationRecordingServerId.ToString();
		SCHmessage msg = CreateConfigurationItemChangedMessage(messageTypeName, hardwareId.ToString(), EventAction.Updated, dictionary);
		return Task.Factory.StartNew(delegate
		{
			try
			{
				SendMessage(msg);
			}
			catch (DatabaseCommunicationException ex)
			{
				logger.LogWarning("Failed to send service channel event {MessageType}. Recorder id: {HardwareId}; Exception: {ExceptionMessage}", messageTypeName, hardwareId, ex.Message);
			}
			catch (ServiceChannelException ex2)
			{
				logger.LogWarning("Failed to send service channel event {MessageType}. Recorder id: {HardwareId}; Exception: {ExceptionMessage}", messageTypeName, hardwareId, ex2.Message);
			}
		}, TaskCreationOptions.LongRunning);
	}

	public Task SendRecordingServerActivatedMessageAsync(Guid recorderId, bool isCreated)
	{
		string messageTypeName = "RecordingServerActivated";
		SCHmessage msg = CreateConfigurationItemChangedMessage(messageTypeName, recorderId.ToString(), (!isCreated) ? EventAction.Updated : EventAction.Added, null);
		return Task.Factory.StartNew(delegate
		{
			try
			{
				SendMessage(msg);
			}
			catch (DatabaseCommunicationException ex)
			{
				logger.LogWarning("Failed to send service channel event {MessageType}. Recorder id: {RecorderId}; Exception: {ExceptionMessage}", messageTypeName, recorderId, ex.Message);
			}
			catch (ServiceChannelException ex2)
			{
				logger.LogWarning("Failed to send service channel event {MessageType}. Recorder id: {RecorderId}; Exception: {ExceptionMessage}", messageTypeName, recorderId, ex2.Message);
			}
		}, TaskCreationOptions.LongRunning);
	}

	public void SendRetrievalTaskChangedMessage(long taskId, EventAction eventAction)
	{
		string text = "RetrievalTaskChanged";
		SCHmessage msg = CreateRetrievalTaskChangedMessage(text, taskId, eventAction);
		try
		{
			SendMessage(msg);
		}
		catch (DatabaseCommunicationException ex)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. task id: {TaskId}; Exception: {ExceptionMessage}", text, taskId, ex.Message);
		}
		catch (ServiceChannelException ex2)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. task id: {TaskId}; Exception: {ExceptionMessage}", text, taskId, ex2.Message);
		}
	}

	public void SendRecordingsChangedMessage(Guid deviceId, DateTime startTime, DateTime endTime)
	{
		string text = "RecordingsChanged";
		SCHmessage msg = CreateDeviceIDWithIntervalMessage(text, deviceId, startTime, endTime);
		try
		{
			SendMessage(msg);
		}
		catch (DatabaseCommunicationException ex)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; startTime: {StartTime}; endTime: {EndTime}; Exception: {ExceptionMessage}", text, deviceId, startTime, endTime, ex.Message);
		}
		catch (ServiceChannelException ex2)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; startTime: {StartTime}; endTime: {EndTime}; Exception: {ExceptionMessage}", text, deviceId, startTime, endTime, ex2.Message);
		}
	}

	public void SendManualRecordingStateMessage(Guid deviceId, bool isManualRecording)
	{
		string text = "ManualRecordingStateChanged";
		SCHmessage msg = CreateManualRecordingStateMessage(text, deviceId, isManualRecording);
		try
		{
			SendMessage(msg);
		}
		catch (DatabaseCommunicationException ex)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; isManualRecording: {IsManualRecording}; Exception: {ExceptionMessage}", text, deviceId, isManualRecording, ex.Message);
		}
		catch (ServiceChannelException ex2)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; isManualRecording: {IsManualRecording}; Exception: {ExceptionMessage}", text, deviceId, isManualRecording, ex2.Message);
		}
	}

	public void SendEdgeJobUpdated(Guid deviceId, DateTime startTime, DateTime endTime)
	{
		string text = "EdgeJobUpdated";
		SCHmessage msg = CreateDeviceIDWithIntervalMessage(text, deviceId, startTime, endTime);
		try
		{
			SendMessage(msg);
		}
		catch (DatabaseCommunicationException ex)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; startTime: {StartTime}; endTime: {EndTime}; Exception: {ExceptionMessage}", text, deviceId, startTime, endTime, ex.Message);
		}
		catch (ServiceChannelException ex2)
		{
			logger.LogWarning("Failed to send service channel event {MessageType}. device id: {DeviceId}; startTime: {StartTime}; endTime: {EndTime}; Exception: {ExceptionMessage}", text, deviceId, startTime, endTime, ex2.Message);
		}
	}

	private static SCHmessage CreateConfigurationItemChangedMessage(string messageTypeName, string configurationItemId, EventAction eventAction, IEnumerable<KeyValuePair<string, string>> additionalMessageDataItems)
	{
		SCHmessage sCHmessage = new SCHmessage();
		sCHmessage.MsgTypeName = messageTypeName;
		List<SCHmessageDataItem> list = new List<SCHmessageDataItem>();
		SCHmessageDataItem item = CreateConfigIdDataItem(configurationItemId);
		list.Add(item);
		SCHmessageDataItem item2 = CreateConfigChangeMessageDataItem(eventAction);
		list.Add(item2);
		if (additionalMessageDataItems != null)
		{
			foreach (KeyValuePair<string, string> additionalMessageDataItem in additionalMessageDataItems)
			{
				SCHmessageDataItem sCHmessageDataItem = new SCHmessageDataItem();
				sCHmessageDataItem.Key = additionalMessageDataItem.Key;
				sCHmessageDataItem.Value = additionalMessageDataItem.Value;
				list.Add(sCHmessageDataItem);
			}
		}
		sCHmessage.DataItems = list.ToArray();
		return sCHmessage;
	}

	private static SCHmessage CreateRetrievalTaskChangedMessage(string messageTypeName, long taskId, EventAction eventAction)
	{
		SCHmessage sCHmessage = new SCHmessage();
		sCHmessage.MsgTypeName = messageTypeName;
		List<SCHmessageDataItem> list = new List<SCHmessageDataItem>();
		SCHmessageDataItem item = CreateConfigChangeMessageDataItem(eventAction);
		list.Add(item);
		SCHmessageDataItem item2 = CreateKeyValueDataItem("RetrievalTaskID", taskId.ToString(CultureInfo.InvariantCulture));
		list.Add(item2);
		sCHmessage.DataItems = list.ToArray();
		return sCHmessage;
	}

	private static SCHmessage CreateDeviceIDWithIntervalMessage(string messageTypeName, Guid deviceId, DateTime startTime, DateTime endTime)
	{
		SCHmessage sCHmessage = new SCHmessage();
		sCHmessage.MsgTypeName = messageTypeName;
		List<SCHmessageDataItem> list = new List<SCHmessageDataItem>();
		SCHmessageDataItem item = CreateKeyValueDataItem("DeviceID", deviceId.ToString());
		list.Add(item);
		SCHmessageDataItem item2 = CreateKeyValueDataItem("StartTime", startTime.ToString("o", CultureInfo.InvariantCulture));
		list.Add(item2);
		SCHmessageDataItem item3 = CreateKeyValueDataItem("EndTime", endTime.ToString("o", CultureInfo.InvariantCulture));
		list.Add(item3);
		sCHmessage.DataItems = list.ToArray();
		return sCHmessage;
	}

	private static SCHmessage CreateManualRecordingStateMessage(string messageTypeName, Guid deviceId, bool isManualRecording)
	{
		SCHmessage sCHmessage = new SCHmessage();
		sCHmessage.MsgTypeName = messageTypeName;
		List<SCHmessageDataItem> list = new List<SCHmessageDataItem>();
		SCHmessageDataItem item = CreateKeyValueDataItem("DeviceID", deviceId.ToString());
		list.Add(item);
		SCHmessageDataItem item2 = CreateKeyValueDataItem("ManualRecordingState", isManualRecording ? "Active" : "Inactive");
		list.Add(item2);
		sCHmessage.DataItems = list.ToArray();
		return sCHmessage;
	}

	private static SCHmessageDataItem CreateConfigIdDataItem(string configurationItemId)
	{
		SCHmessageDataItem sCHmessageDataItem = new SCHmessageDataItem();
		sCHmessageDataItem.Key = "ConfigID";
		sCHmessageDataItem.Value = configurationItemId;
		return sCHmessageDataItem;
	}

	private static SCHmessageDataItem CreateKeyValueDataItem(string key, string value)
	{
		SCHmessageDataItem sCHmessageDataItem = new SCHmessageDataItem();
		sCHmessageDataItem.Key = key;
		sCHmessageDataItem.Value = value;
		return sCHmessageDataItem;
	}

	private static SCHmessageDataItem CreateConfigChangeMessageDataItem(EventAction eventAction)
	{
		SCHmessageDataItem sCHmessageDataItem = new SCHmessageDataItem();
		sCHmessageDataItem.Key = "ConfigChange";
		switch (eventAction)
		{
		case EventAction.Added:
			sCHmessageDataItem.Value = "Added";
			break;
		case EventAction.Updated:
			sCHmessageDataItem.Value = "Updated";
			break;
		case EventAction.Deleted:
			sCHmessageDataItem.Value = "Deleted";
			break;
		}
		return sCHmessageDataItem;
	}

	private void SendMessage(SCHmessage msg)
	{
		int num = 2;
		do
		{
			try
			{
				if (client == null)
				{
					Uri serviceAddress = GetServiceAddress(Uri.UriSchemeHttp, serverPort);
					client = GetServiceChannelClient(serviceAddress);
				}
			}
			catch (ServiceAddressInvalidException)
			{
				throw;
			}
			num--;
			try
			{
				if (logger.IsEnabled(LogLevel.Debug))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(msg.MsgTypeName + " (" + msg.MessageID.ToString(CultureInfo.InvariantCulture) + ")");
					if (msg.DataItems != null)
					{
						SCHmessageDataItem[] dataItems = msg.DataItems;
						foreach (SCHmessageDataItem sCHmessageDataItem in dataItems)
						{
							stringBuilder.Append(Environment.NewLine + "  " + sCHmessageDataItem.Key + ", " + sCHmessageDataItem.Value);
						}
					}
					logger.LogDebug("SCHMessage: {SchMessage}", stringBuilder.ToString());
				}
				client.CreateMessages(new SCHmessage[1] { msg });
				num = 0;
			}
			catch (CommunicationException inner)
			{
				if (client != null)
				{
					client.Abort();
					client = null;
				}
				if (num == 0)
				{
					throw new ServiceChannelCommunicationException("Communication with the service channel service failed.", inner);
				}
			}
			catch (TimeoutException inner2)
			{
				if (client != null)
				{
					client.Abort();
					client = null;
				}
				if (num == 0)
				{
					throw new ServiceChannelCommunicationException("Communication with the service channel service timed out.", inner2);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception)
			{
				if (client != null)
				{
					client.Abort();
					client = null;
				}
				if (num == 0)
				{
					throw;
				}
			}
		}
		while (num > 0);
	}

	public static SCHMessageCreatorManagerClient GetServiceChannelClient(Uri serviceAddress)
	{
		EndpointIdentity serviceIdentity = GetServiceIdentity(serviceAddress);
		EndpointAddress remoteAddress = new EndpointAddress(serviceAddress, serviceIdentity);
		WSHttpBinding wSHttpBinding = new WSHttpBinding();
		WSHttpSecurity security = wSHttpBinding.Security;
		security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.MaxBufferPoolSize = 524288L;
		wSHttpBinding.MaxReceivedMessageSize = 2147483647L;
		if (Debugger.IsAttached)
		{
			wSHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(5.0);
			wSHttpBinding.SendTimeout = TimeSpan.FromMinutes(5.0);
		}
		wSHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		SCHMessageCreatorManagerClient sCHMessageCreatorManagerClient = new SCHMessageCreatorManagerClient(wSHttpBinding, remoteAddress);
		sCHMessageCreatorManagerClient.ClientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
		return sCHMessageCreatorManagerClient;
	}

	private static EndpointIdentity GetServiceIdentity(Uri serviceUrl)
	{
		string spn = SpnFactory.GetSpn(serviceUrl);
		return EndpointIdentity.CreateSpnIdentity(spn);
	}

	private static Uri GetServiceAddress(string scheme, int port)
	{
		string text = GetServiceChannelBaseUrl(scheme, port);
		if (string.IsNullOrEmpty(text))
		{
			throw new ServiceAddressInvalidException("The address for the registered service channel is empty.");
		}
		if (!text.EndsWith("/", StringComparison.OrdinalIgnoreCase))
		{
			text += "/";
		}
		text += "SCHMessageCreatorManager.1.svc";
		return new Uri(text);
	}

	private static string GetServiceChannelBaseUrl(string scheme, int port)
	{
		UriBuilder uriBuilder = new UriBuilder
		{
			Scheme = scheme,
			Host = Dns.GetHostName(),
			Path = "ManagementServer",
			Port = port
		};
		return uriBuilder.Uri.ToString();
	}
}
