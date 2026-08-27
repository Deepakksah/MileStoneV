using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.AlarmClient;

public class AlarmClientManager
{
	private Dictionary<ServerId, IAlarmClient> _clients = new Dictionary<ServerId, IAlarmClient>();

	private Dictionary<ServerId, Configuration.ServiceURIInfo> _serviceUris = new Dictionary<ServerId, Configuration.ServiceURIInfo>();

	private object _lockObject = new object();

	public IAlarmClient GetAlarmClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			ServerId serverId2 = serverId;
			if (serverId2 == null)
			{
				throw new MIPException("AlarmClient not created: ServerId cannot be null");
			}
			if (serverId2.ServerType == "XPCORS")
			{
				serverId2 = GetParentServerId(serverId2);
				if (serverId2 == null)
				{
					throw new MIPException("AlarmClient not created: ServerId from a Recorder has no Parent?");
				}
			}
			IAlarmClient alarmClient2;
			if (!_clients.ContainsKey(serverId2))
			{
				IAlarmClient alarmClient = CreateAlarmCommandClient(serverId2);
				if (alarmClient == null)
				{
					throw new MIPException("Alarm Client not created: Unable to find EventServer via Registered Services (" + serverId2.ServerHostname + ":" + serverId2.Serverport.ToString(CultureInfo.InvariantCulture) + ")");
				}
				_clients.Add(serverId2, alarmClient);
			}
			else
			{
				alarmClient2 = _clients[serverId2];
				if (alarmClient2.State == CommunicationState.Closed || alarmClient2.State == CommunicationState.Closing || alarmClient2.State == CommunicationState.Faulted)
				{
					_clients.Remove(serverId2);
					IAlarmClient alarmClient3 = CreateAlarmCommandClient(serverId2);
					if (alarmClient3 == null)
					{
						throw new MIPException("Alarm Client not created: EventServer or ImageServer might not be available");
					}
					_clients.Add(serverId2, alarmClient3);
				}
			}
			alarmClient2 = _clients[serverId2];
			if (alarmClient2.State == CommunicationState.Created)
			{
				alarmClient2.OpenClient();
			}
			return alarmClient2;
		}
	}

	public void Clear(ServerId serverId)
	{
		lock (_lockObject)
		{
			if (_clients.ContainsKey(serverId))
			{
				try
				{
					_clients[serverId].CloseClient();
				}
				catch
				{
				}
				_clients.Remove(serverId);
			}
		}
	}

	public void Clear()
	{
		lock (_lockObject)
		{
			try
			{
				foreach (IAlarmClient value in _clients.Values)
				{
					value.CloseClient();
				}
				_clients.Clear();
			}
			catch
			{
			}
		}
	}

	public void Close()
	{
		Clear();
	}

	private static ServerId GetParentServerId(ServerId serverId)
	{
		Item item = ((serverId.UserContext == null) ? Configuration.Instance.GetItem(serverId.Id, Kind.Server) : serverId.UserContext.Configuration.GetItem(serverId.Id, Kind.Server));
		if (item != null)
		{
			Item parent = item.GetParent();
			if (parent != null)
			{
				return parent.FQID.ServerId;
			}
		}
		return null;
	}

	private IAlarmClient CreateAlarmCommandClient(ServerId serverId)
	{
		try
		{
			Configuration configuration = ((serverId.UserContext == null) ? Configuration.Instance : serverId.UserContext.Configuration);
			List<Configuration.ServiceURIInfo> registeredServiceUriInfo = configuration.GetRegisteredServiceUriInfo(Configuration.MAPServiceId, serverId);
			if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
			{
				throw new MIPException("Event server not found in registered services");
			}
			bool useToken = LoginSettingsCache.GetLoginSettings(serverId)?.IsOAuthConnection ?? false;
			_serviceUris[serverId] = registeredServiceUriInfo[0];
			return AlarmClientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], GetEventServerVersion(registeredServiceUriInfo[0]), useToken);
		}
		catch (ThreadAbortException)
		{
			return null;
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: true, "AlarmClientManager", string.Format(CultureInfo.InvariantCulture, "Failed to create alarm client. Exception: {0} ", ex2.Message), null);
			return null;
		}
	}

	private static Version GetEventServerVersion(Configuration.ServiceURIInfo eventServerURIInfo)
	{
		if (!string.IsNullOrEmpty(eventServerURIInfo.ServiceData))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(eventServerURIInfo.ServiceData);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("EventServer/Version");
			if (xmlNode != null && xmlNode.HasChildNodes && !string.IsNullOrEmpty(xmlNode.InnerText))
			{
				Match match = Regex.Match(xmlNode.InnerText, "^(?<major>\\d+)\\.(?<minor>\\d+)(?<revision>[a-z]?)$");
				if (match.Success)
				{
					int minor = 0;
					int revision = 0;
					try
					{
						minor = int.Parse(match.Groups["minor"].Value, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
					}
					try
					{
						revision = Math.Max(match.Groups["revision"].Value[0] - 97, 0);
					}
					catch (Exception)
					{
					}
					return new Version(int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture), minor, 0, revision);
				}
			}
		}
		return null;
	}
}
