using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.EventServer.Communication;
using VideoOS.EventServer.Communication.Configuration;
using VideoOS.EventServer.Communication.Map;
using VideoOS.EventServer.Communication.Map.Client;
using VideoOS.Management.Common;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

public class AlarmDefinitionConfigurationManager
{
	private static AlarmDefinitionConfigurationManager _instance = null;

	private static readonly object _instanceLock = new object();

	private ConfigurationClientManager _clientManager = new ConfigurationClientManager();

	private const int _bindingValue = 500000000;

	private const int _maxItems = 1000000;

	private bool _connected;

	private object _connectedLock = new object();

	private readonly Guid MAP_TYPE = new Guid("da39f4ca-20fc-48ab-82cf-8508bbb7e67a");

	public static AlarmDefinitionConfigurationManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_instanceLock)
				{
					if (_instance == null)
					{
						_instance = new AlarmDefinitionConfigurationManager();
						_instance.Connect();
					}
				}
			}
			return _instance;
		}
	}

	public static bool HasInstance
	{
		get
		{
			if (_instance != null)
			{
				return true;
			}
			return false;
		}
	}

	private bool IsConnected
	{
		get
		{
			return _connected;
		}
		set
		{
			bool flag = false;
			lock (_connectedLock)
			{
				if (_connected != value)
				{
					_connected = value;
					if (_connected)
					{
						EnvironmentManager.Instance.Log(error: false, "ServerStatus", "Connected to the event server.", null);
					}
					else
					{
						EnvironmentManager.Instance.Log(error: false, "ServerStatus", "Event server connection lost.", null);
					}
					flag = true;
				}
			}
			if (flag && this.ConnectionStateChanged != null)
			{
				this.ConnectionStateChanged(this, new EventArgs());
			}
		}
	}

	public event EventHandler ConnectionStateChanged;

	private AlarmDefinitionConfigurationManager()
	{
	}

	public void Close()
	{
		_clientManager.Close();
	}

	private T CallWithTryCatch<T>(Func<ConfigurationClient, T> func, string logWhere, string logText, bool showError = true)
	{
		try
		{
			ConfigurationClient configurationClient = _clientManager.GetConfigurationClient(EnvironmentManager.Instance.CurrentSite.ServerId);
			return (configurationClient == null) ? default(T) : func(configurationClient);
		}
		catch (Exception e)
		{
			HandleException(e, logWhere, logText, showError);
			return default(T);
		}
	}

	private void CallWithTryCatch(Action<ConfigurationClient> func, string logWhere, string logText, bool showError = true)
	{
		try
		{
			ConfigurationClient configurationClient = _clientManager.GetConfigurationClient(EnvironmentManager.Instance.CurrentSite.ServerId);
			func(configurationClient);
		}
		catch (Exception e)
		{
			HandleException(e, logWhere, logText, showError);
		}
	}

	private void HandleException(Exception e, string logWhere, string logText, bool showError)
	{
		if (showError && !string.IsNullOrEmpty(logWhere) && !string.IsNullOrEmpty(logText))
		{
			string text = e.Message;
			if (e.InnerException != null)
			{
				text = text + Environment.NewLine + "InnerException: " + e.InnerException.Message;
			}
			EnvironmentManager.Instance.Log(error: true, logWhere, logText + ": " + text, null);
		}
		IsConnected = false;
	}

	private bool EnsureConnected()
	{
		if (!IsConnected)
		{
			lock (_connectedLock)
			{
				if (!IsConnected)
				{
					Connect();
				}
			}
		}
		return IsConnected;
	}

	public GeneralSettings GetGeneralSettings(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch((ConfigurationClient client) => client.GetGeneralSettings(), "GeneralSettings", "Failed to get general settings");
		}
		return null;
	}

	public bool SetGeneralSettings(IGeminiRequestContext requestContext, GeneralSettings settings)
	{
		VerifyWriteAccess(requestContext);
		return CallWithTryCatch(delegate(ConfigurationClient client)
		{
			client.SetGeneralSettings(settings);
			return true;
		}, "GeneralSettings", "Failed to save general settings");
	}

	public EventRetentions GetEventRetentions(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		if (!EnsureConnected())
		{
			return null;
		}
		return CallWithTryCatch((ConfigurationClient client) => client.GetEventRetention(new GetEventRetentionParams())?.EventRetentions, "EventRetentions", "Failed to get event retentions");
	}

	public EventRetentions GetDefaultEventRetentions(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		if (!EnsureConnected())
		{
			return null;
		}
		return CallWithTryCatch((ConfigurationClient client) => client.GetDefaultEventRetention(new GetDefaultEventRetentionParams())?.EventRetentions, "EventRetentions", "Failed to get default event retentions");
	}

	public AlarmDefinition[] GetAlarmDefinitions(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch((ConfigurationClient client) => client.GetAlarmDefinitions(), "ConfigAPI.AlarmDefinitions", "Failed to get alarm definitions");
		}
		return null;
	}

	public AlarmDefinition GetAlarmDefinition(IGeminiRequestContext requestContext, Guid id)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch((ConfigurationClient client) => client.GetAlarmDefinition(id), "ConfigAPI.AlarmDefinition", "Failed to get alarm definition");
		}
		return null;
	}

	public void SaveAlarmDefinition(IGeminiRequestContext requestContext, AlarmDefinition alarmDefinition)
	{
		VerifyWriteAccess(requestContext);
		CallWithTryCatch(delegate(ConfigurationClient client)
		{
			if (!client.UpdateAlarmDefinition(alarmDefinition))
			{
				throw new ArgumentException("Unable to update alarm definition");
			}
		}, "AlarmDefinitions", "Failed to save alarm definition");
	}

	public AlarmDefinition CreateAlarmDefinition(IGeminiRequestContext requestContext, string name)
	{
		VerifyWriteAccess(requestContext);
		return CallWithTryCatch(delegate(ConfigurationClient client)
		{
			AlarmDefinition alarmDefinition = new AlarmDefinition
			{
				Id = Guid.NewGuid(),
				Name = name,
				Enabled = true,
				Priority = AlarmDefinitionConstants.PriorityHigh,
				EventType = "",
				AlarmActions = new AlarmAction[0]
			};
			client.CreateAlarmDefinition(alarmDefinition);
			return alarmDefinition;
		}, "AlarmDefinitions", "Failed to create alarm definition");
	}

	public bool DeleteAlarmDefinition(IGeminiRequestContext requestContext, Guid id)
	{
		VerifyWriteAccess(requestContext);
		return CallWithTryCatch((ConfigurationClient client) => client.DeleteAlarmDefinition(id), "AlarmDefinitions", "Failed to delete alarm definition");
	}

	public List<EventType> GetEventTypes(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch(delegate(ConfigurationClient client)
			{
				int? eventServerVersion = ServerReceiver.GetEventServerVersion(EnvironmentManager.Instance.CurrentSite.ServerId);
				return (eventServerVersion.HasValue && eventServerVersion >= 3) ? new List<EventType>(client.GetEventTypes()) : new List<EventType>();
			}, "ConfigurationManager", "Failed to get event types");
		}
		return null;
	}

	public List<EventType> GetEventTypes(IGeminiRequestContext requestContext, CultureInfo culture)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch(delegate(ConfigurationClient client)
			{
				int? eventServerVersion = ServerReceiver.GetEventServerVersion(EnvironmentManager.Instance.CurrentSite.ServerId);
				return (eventServerVersion.HasValue && eventServerVersion.Value >= 3) ? new List<EventType>(client.GetEventTypesTranslated(culture)) : new List<EventType>();
			}, "ConfigurationManager", "Failed to get event types");
		}
		return null;
	}

	public List<EventTypeGroup> GetEventTypeGroups(IGeminiRequestContext requestContext, CultureInfo culture = null, bool replaceAccessControlCategories = false)
	{
		VerifyReadAccess(requestContext);
		if (EnsureConnected())
		{
			return CallWithTryCatch(delegate(ConfigurationClient client)
			{
				if (culture != null)
				{
					CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
					Thread.CurrentThread.CurrentUICulture = culture;
					culture = currentUICulture;
				}
				try
				{
					EventTypeGroup[] eventTypeGroups = client.GetEventTypeGroups();
					if (replaceAccessControlCategories)
					{
						EventTypeGroup eventTypeGroup = eventTypeGroups.FirstOrDefault((EventTypeGroup group) => group.ID == AlarmDefinitionConstants.AccessControlCategoryEventGroup);
						if (eventTypeGroup != null)
						{
							eventTypeGroup.ID = AlarmDefinitionConstants.AccessControlEventGroup;
							eventTypeGroup.Name = "Access Control Events";
						}
					}
					return new List<EventTypeGroup>(eventTypeGroups);
				}
				finally
				{
					if (culture != null)
					{
						Thread.CurrentThread.CurrentUICulture = culture;
					}
				}
			}, "ConfigurationManager", "Failed to get event type groups");
		}
		return null;
	}

	public bool Connect()
	{
		if (EnvironmentManager.Instance.CurrentSite.ServerId.Uri.Host.Contains("http://fakeaddress"))
		{
			IsConnected = false;
			return IsConnected;
		}
		try
		{
			_clientManager.GetConfigurationClient(EnvironmentManager.Instance.CurrentSite.ServerId).GetGeneralSettings();
			IsConnected = true;
		}
		catch (Exception)
		{
			IsConnected = false;
		}
		return IsConnected;
	}

	public static string GetUniqueItemName(string baseName, string baseNameNum, IEnumerable<Item> items)
	{
		int num = 0;
		string text = baseName;
		bool flag = true;
		while (flag)
		{
			flag = false;
			if (items == null)
			{
				continue;
			}
			foreach (Item item in items)
			{
				if (item.Name.Equals(text))
				{
					flag = true;
					text = string.Format(CultureInfo.InvariantCulture, baseNameNum, ++num);
					break;
				}
			}
		}
		return text;
	}

	private static void VerifyReadAccess(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.Alarms, "GENERIC_READ");
	}

	private static void VerifyWriteAccess(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckOverallPermission(requestContext, SecurityNamespaceIdentifier.Alarms, "GENERIC_WRITE");
	}

	public AlarmDataSettings GetAlarmDataSettings(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		return CallWithTryCatch((ConfigurationClient client) => client.GetAlarmDataSettings(), "AlarmDataSettings", "Failed to get alarm data settings");
	}

	public Dictionary<string, MapUpdateData> GetMapList(IGeminiRequestContext requestContext)
	{
		VerifyReadAccess(requestContext);
		try
		{
			MapClientManager mapClientManager = new MapClientManager();
			MapClient mapClient = mapClientManager.GetMapClient(EnvironmentManager.Instance.MasterSite.ServerId);
			SimpleVisualElement[] allObjectsContaining = mapClient.GetAllObjectsContaining(MAP_TYPE, null);
			Dictionary<string, MapUpdateData> dictionary = new Dictionary<string, MapUpdateData>();
			SimpleVisualElement[] array = allObjectsContaining;
			foreach (SimpleVisualElement simpleVisualElement in array)
			{
				if (simpleVisualElement != null && simpleVisualElement.Id != null && !string.IsNullOrWhiteSpace(simpleVisualElement.Id.ID))
				{
					string iD = simpleVisualElement.Id.ID;
					if ((simpleVisualElement.Id.ServerID.Equals(Guid.Empty) || simpleVisualElement.Id.ServerID.Equals(EnvironmentManager.Instance.MasterSite.ServerId.Id)) && !dictionary.ContainsKey(iD))
					{
						dictionary.Add(iD, new MapUpdateData
						{
							Id = iD,
							Name = simpleVisualElement.Name,
							ServerId = EnvironmentManager.Instance.MasterSite.ServerId,
							ServerOrder = 1
						});
					}
				}
			}
			mapClientManager.Close();
			mapClient.Close();
			return dictionary;
		}
		catch (Exception ex)
		{
			HandleException(ex, "AlarmDefinitionConfigurationManager", ex.Message, showError: true);
			return new Dictionary<string, MapUpdateData>();
		}
	}
}
