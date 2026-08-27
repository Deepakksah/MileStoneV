using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClientProfile : IConfigurationItem
{
	public ICollection<ClientProfileAccessControlChildItem> ClientProfileAccessControlChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileAccessControlChildItem> list = new List<ClientProfileAccessControlChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileAccessControl")
				{
					list.Add(new ClientProfileAccessControlChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileAdvancedChildItem> ClientProfileAdvancedChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileAdvancedChildItem> list = new List<ClientProfileAdvancedChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileAdvanced")
				{
					list.Add(new ClientProfileAdvancedChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileAlarmManagerChildItem> ClientProfileAlarmManagerChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileAlarmManagerChildItem> list = new List<ClientProfileAlarmManagerChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileAlarmManager")
				{
					list.Add(new ClientProfileAlarmManagerChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileExportChildItem> ClientProfileExportChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileExportChildItem> list = new List<ClientProfileExportChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileExport")
				{
					list.Add(new ClientProfileExportChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileGeneralChildItem> ClientProfileGeneralChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileGeneralChildItem> list = new List<ClientProfileGeneralChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileGeneral")
				{
					list.Add(new ClientProfileGeneralChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileGisMapChildItem> ClientProfileGisMapChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileGisMapChildItem> list = new List<ClientProfileGisMapChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileGisMap")
				{
					list.Add(new ClientProfileGisMapChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileLiveChildItem> ClientProfileLiveChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileLiveChildItem> list = new List<ClientProfileLiveChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileLive")
				{
					list.Add(new ClientProfileLiveChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfilePlaybackChildItem> ClientProfilePlaybackChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfilePlaybackChildItem> list = new List<ClientProfilePlaybackChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfilePlayback")
				{
					list.Add(new ClientProfilePlaybackChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileSetupChildItem> ClientProfileSetupChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileSetupChildItem> list = new List<ClientProfileSetupChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileSetup")
				{
					list.Add(new ClientProfileSetupChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileTimelineChildItem> ClientProfileTimelineChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileTimelineChildItem> list = new List<ClientProfileTimelineChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileTimeline")
				{
					list.Add(new ClientProfileTimelineChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ICollection<ClientProfileViewLayoutsChildItem> ClientProfileViewLayoutsChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClientProfileViewLayoutsChildItem> list = new List<ClientProfileViewLayoutsChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ClientProfileViewLayouts")
				{
					list.Add(new ClientProfileViewLayoutsChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool IsDefaultProfile
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IsDefaultProfile");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public ClientProfile(ServerId serverId, ConfigurationItem clientProfile)
		: base(serverId, clientProfile)
	{
	}

	public ClientProfile(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClientProfile(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ClientProfileUpPriority()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ClientProfileUpPriority").ExecuteMethod("ClientProfileUpPriority"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfile", "Unable to call method 'ClientProfileUpPriority':" + ex.Message);
		}
		return null;
	}

	public ServerTask ClientProfileDownPriority()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ClientProfileDownPriority").ExecuteMethod("ClientProfileDownPriority"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClientProfile", "Unable to call method 'ClientProfileDownPriority':" + ex.Message);
		}
		return null;
	}
}
