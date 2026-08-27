using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MIPKindFolder : IConfigurationItem
{
	public ICollection<MIPKind> MIPKinds
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<MIPKind> list = new List<MIPKind>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "MIPKind")
				{
					list.Add(new MIPKind(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MIPKindFolder(ServerId serverId)
		: base(serverId, "/MIPKindFolder")
	{
	}

	public ServerTask AddMIPKind(string id, string name, string pluginId, string pluginName, string kindType, bool displayOnGisMap, Guid parentKind, string securityAction, string securityActionNames)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMIPKind");
			configurationApiItemWrapper.SetProperty("Id", id);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("PluginId", pluginId);
			configurationApiItemWrapper.SetProperty("PluginName", pluginName);
			configurationApiItemWrapper.SetProperty("KindType", kindType);
			configurationApiItemWrapper.SetProperty("DisplayOnGisMap", displayOnGisMap.ToString());
			configurationApiItemWrapper.SetProperty("ParentKind", parentKind.ToString());
			configurationApiItemWrapper.SetProperty("SecurityAction", securityAction);
			configurationApiItemWrapper.SetProperty("SecurityActionNames", securityActionNames);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMIPKind"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPKindFolder", "Unable to call method 'AddMIPKind':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveMIPKind(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMIPKind");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMIPKind"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPKindFolder", "Unable to call method 'RemoveMIPKind':" + ex.Message);
		}
		return null;
	}

	public MIPOptionProperties GetMipOptionProperties(Guid optionId, bool userPrivate)
	{
		ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("GetMIPOptionProperty");
		configurationApiItemWrapper.SetProperty("OptionId", optionId.ToString());
		configurationApiItemWrapper.SetProperty("UserPrivate", userPrivate.ToString());
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper2 = configurationApiItemWrapper.ExecuteMethod("GetMIPOptionProperty");
			string propertyValue = configurationApiItemWrapper2.GetPropertyValue("DataVersion");
			string propertyValue2 = configurationApiItemWrapper2.GetPropertyValue("XML");
			if (propertyValue != null && propertyValue2 != null && int.TryParse(propertyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				return new MIPOptionProperties
				{
					OptionId = optionId,
					DataVersion = result,
					PropertiesXml = propertyValue2
				};
			}
		}
		catch (PathNotFoundMIPException)
		{
			return null;
		}
		return null;
	}

	public void SetMipOptionProperties(MIPOptionProperties mipOptionProperties, bool userPrivate)
	{
		if (mipOptionProperties == null)
		{
			throw new ArgumentNullException("mipOptionProperties");
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SetMIPOptionProperty");
		configurationApiItemWrapper.SetProperty("OptionId", mipOptionProperties.OptionId.ToString());
		configurationApiItemWrapper.SetProperty("DataVersion", mipOptionProperties.DataVersion.ToString(CultureInfo.InvariantCulture));
		configurationApiItemWrapper.SetProperty("XML", mipOptionProperties.PropertiesXml);
		configurationApiItemWrapper.SetProperty("UserPrivate", userPrivate ? "True" : "False");
		configurationApiItemWrapper.ExecuteMethod("SetMIPOptionProperty");
	}

	public int GetDataVersionSum()
	{
		if (int.TryParse(base.Wrapper.ExecuteMethod("MIPItemDataVersionSum").GetPropertyValue("MIPItemDataVersionSum"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return 0;
	}

	public MIPOptionProperties GetMipOptionProperties(Guid optionId)
	{
		ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("GetMIPOptionProperty");
		configurationApiItemWrapper.SetProperty("OptionId", optionId.ToString());
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper2 = configurationApiItemWrapper.ExecuteMethod("GetMIPOptionProperty");
			string propertyValue = configurationApiItemWrapper2.GetPropertyValue("DataVersion");
			string propertyValue2 = configurationApiItemWrapper2.GetPropertyValue("XML");
			if (propertyValue != null && propertyValue2 != null && int.TryParse(propertyValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				return new MIPOptionProperties
				{
					OptionId = optionId,
					DataVersion = result,
					PropertiesXml = propertyValue2
				};
			}
		}
		catch (PathNotFoundMIPException)
		{
			return null;
		}
		return null;
	}

	public void SetMipOptionProperties(MIPOptionProperties mipOptionProperties)
	{
		if (mipOptionProperties == null)
		{
			throw new ArgumentNullException("mipOptionProperties");
		}
		ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SetMIPOptionProperty");
		configurationApiItemWrapper.SetProperty("OptionId", mipOptionProperties.OptionId.ToString());
		configurationApiItemWrapper.SetProperty("DataVersion", mipOptionProperties.DataVersion.ToString(CultureInfo.InvariantCulture));
		configurationApiItemWrapper.SetProperty("XML", mipOptionProperties.PropertiesXml);
		configurationApiItemWrapper.ExecuteMethod("SetMIPOptionProperty");
	}

	public ServerTask AddMIPKind(string id, string name, string pluginId, string pluginName, string kindType, bool displayOnGisMap, Guid parentKind, string securityAction)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMIPKind");
			configurationApiItemWrapper.SetProperty("Id", id);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("PluginId", pluginId);
			configurationApiItemWrapper.SetProperty("PluginName", pluginName);
			configurationApiItemWrapper.SetProperty("KindType", kindType);
			configurationApiItemWrapper.SetProperty("DisplayOnGisMap", displayOnGisMap.ToString());
			configurationApiItemWrapper.SetProperty("ParentKind", parentKind.ToString());
			configurationApiItemWrapper.SetProperty("SecurityAction", securityAction);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMIPKind"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MIPKindFolder", "Unable to call method 'AddMIPKind':" + ex.Message);
		}
		return null;
	}
}
