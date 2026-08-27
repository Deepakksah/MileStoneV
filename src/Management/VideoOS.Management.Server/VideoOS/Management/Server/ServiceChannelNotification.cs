using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class ServiceChannelNotification
{
	private EventObject _eventObject;

	public event EventHandler NotificationDelivered;

	public ServiceChannelNotification(EventObject eventObject)
	{
		_eventObject = eventObject;
	}

	private static string GetServiceChannelBaseUrl(GeminiRequestContext systemRequestContext)
	{
		return new UriBuilder(new Uri((ServerInformation.GetServerInformation(systemRequestContext).Addresses.FirstOrDefault((ServerAddress a) => a.Type == ServerAddressType.Server) ?? throw new ManagementException("The address for the management server could not be found.")).URL))
		{
			Path = "ManagementServer"
		}.Uri.ToString();
	}

	public static SCHMessageCreatorManagerClient CreateClient(string serviceChannelAddress)
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding();
		wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
		wSHttpBinding.MaxBufferPoolSize = 524288L;
		wSHttpBinding.MaxReceivedMessageSize = 2147483647L;
		wSHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
		if (Debugger.IsAttached)
		{
			wSHttpBinding.ReceiveTimeout = TimeSpan.FromMinutes(5.0);
			wSHttpBinding.SendTimeout = TimeSpan.FromMinutes(5.0);
		}
		if (!serviceChannelAddress.EndsWith("/", StringComparison.OrdinalIgnoreCase))
		{
			serviceChannelAddress += "/";
		}
		serviceChannelAddress += "SCHMessageCreatorManager.1.svc";
		UriBuilder uriBuilder = new UriBuilder(serviceChannelAddress);
		EndpointAddress endpointAddress = new EndpointAddress(identity: EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(uriBuilder.Uri)), uri: uriBuilder.Uri, addressHeaders: Array.Empty<AddressHeader>());
		GeminiTrace.Verbose("Service channel address: '{0}'.", endpointAddress.Uri.ToString());
		SCHMessageCreatorManagerClient sCHMessageCreatorManagerClient = new SCHMessageCreatorManagerClient(wSHttpBinding, endpointAddress);
		sCHMessageCreatorManagerClient.ClientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
		return sCHMessageCreatorManagerClient;
	}

	public void Send(EventHandler handler)
	{
		if (handler != null)
		{
			this.NotificationDelivered = (EventHandler)Delegate.Combine(this.NotificationDelivered, handler);
		}
		try
		{
			using GeminiRequestContext systemRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
			string serviceChannelBaseUrl = GetServiceChannelBaseUrl(systemRequestContext);
			if (string.IsNullOrEmpty(serviceChannelBaseUrl) || GeminiApplicationCore.IsExecutingModuleTest)
			{
				return;
			}
			using (SCHMessageCreatorManagerClient sCHMessageCreatorManagerClient = CreateClient(serviceChannelBaseUrl))
			{
				SCHmessage sCHmessage = new SCHmessage();
				sCHmessage.MsgTypeName = _eventObject.EventTypeName;
				List<SCHmessageDataItem> list = new List<SCHmessageDataItem>();
				SCHmessageDataItem sCHmessageDataItem = new SCHmessageDataItem();
				sCHmessageDataItem.Key = "ConfigID";
				sCHmessageDataItem.Value = _eventObject.EventObjectId;
				list.Add(sCHmessageDataItem);
				SCHmessageDataItem sCHmessageDataItem2 = new SCHmessageDataItem();
				sCHmessageDataItem2.Key = "ConfigChange";
				switch (_eventObject.Action)
				{
				case EventAction.Added:
					sCHmessageDataItem2.Value = "Added";
					break;
				case EventAction.Updated:
					sCHmessageDataItem2.Value = "Updated";
					break;
				case EventAction.Deleted:
					sCHmessageDataItem2.Value = "Deleted";
					break;
				}
				list.Add(sCHmessageDataItem2);
				foreach (string key in _eventObject._eventData.Keys)
				{
					SCHmessageDataItem sCHmessageDataItem3 = new SCHmessageDataItem();
					sCHmessageDataItem3.Key = key;
					sCHmessageDataItem3.Value = _eventObject._eventData[key];
					list.Add(sCHmessageDataItem3);
				}
				sCHmessage.DataItems = list.ToArray();
				sCHMessageCreatorManagerClient.CreateMessages(new SCHmessage[1] { sCHmessage });
			}
			GeminiTrace.Verbose("Event has been sent to service channel: '{0}'.", _eventObject.EventTypeName);
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("Failed to send Event to service channel: '{0}'. Failed with exception: {1}", _eventObject.EventTypeName, ex.ToString());
		}
	}

	public static int Save()
	{
		return 0;
	}
}
