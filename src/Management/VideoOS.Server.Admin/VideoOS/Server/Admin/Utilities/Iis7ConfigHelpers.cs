using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Web.Administration;

namespace VideoOS.Server.Admin.Utilities;

public static class Iis7ConfigHelpers
{
	public static (ConfigurationElement virtualDirectory, ConfigurationElement parentApplication) FindVirtualDirectoryConfiguration(string siteName, string virtualDirectoryName, ServerManager serverManager)
	{
		return FindVirtualDirectoryConfiguration(GetSiteConfiguration(siteName, serverManager) ?? throw new InvalidOperationException("Element not found!"), virtualDirectoryName);
	}

	public static (ConfigurationElement virtualDirectory, ConfigurationElement parentApplication) FindVirtualDirectoryConfiguration(ConfigurationElement siteConfiguration, string applicationName)
	{
		string text = "/";
		string text2 = "/" + applicationName;
		foreach (ConfigurationElement item in FindElements(siteConfiguration.GetCollection(), ElementTag.Application, AttributeName.Path, text2))
		{
			ConfigurationElement configurationElement = FindElement(item.GetCollection(), ElementTag.VirtualDirectory, AttributeName.Path, text);
			if (configurationElement != null)
			{
				return (virtualDirectory: configurationElement, parentApplication: item);
			}
		}
		return (virtualDirectory: null, parentApplication: null);
	}

	public static (ConfigurationElement virtualDirectory, ConfigurationElement parentApplication) CreateVirtualDirectory(ConfigurationElement siteConfiguration, string applicationName)
	{
		string value = "/";
		string applicationPath = "/" + applicationName;
		ConfigurationElementCollection collection = siteConfiguration.GetCollection();
		ConfigurationElement configurationElement = collection.FirstOrDefault((ConfigurationElement x) => ((string)x[AttributeName.Path]).Equals(applicationPath, StringComparison.OrdinalIgnoreCase));
		bool num = configurationElement != null;
		if (!num)
		{
			configurationElement = collection.CreateElement("application");
			configurationElement[AttributeName.Path] = applicationPath;
		}
		ConfigurationElementCollection collection2 = configurationElement.GetCollection();
		ConfigurationElement configurationElement2 = collection2.CreateElement(ElementTag.VirtualDirectory);
		configurationElement2[AttributeName.Path] = value;
		collection2.Add(configurationElement2);
		if (!num)
		{
			collection.Add(configurationElement);
		}
		return (virtualDirectory: configurationElement2, parentApplication: configurationElement);
	}

	public static void DeleteVirtualDirectory(ConfigurationElement siteConfiguration, ConfigurationElement parentApplication)
	{
		ConfigurationElementCollection collection = siteConfiguration.GetCollection();
		parentApplication.GetCollection().Clear();
		int num = IndexOf(collection, parentApplication);
		if (num >= 0)
		{
			collection.RemoveAt(num);
		}
	}

	private static int IndexOf(ConfigurationElementCollection applicationCollection, ConfigurationElement applicationElement)
	{
		for (int i = 0; i < applicationCollection.Count; i++)
		{
			if (ApplicationConfigurationElementsAreEqual(applicationCollection[i], applicationElement))
			{
				return i;
			}
		}
		return -1;
	}

	private static bool ApplicationConfigurationElementsAreEqual(ConfigurationElement elementA, ConfigurationElement elementB)
	{
		if (elementA.Attributes.Count != elementB.Attributes.Count)
		{
			return false;
		}
		for (int i = 0; i < elementA.Attributes.Count; i++)
		{
			if (!elementA.Attributes[i].Name.Equals(elementB.Attributes[i].Name))
			{
				return false;
			}
			if (!elementA.Attributes[i].Value.Equals(elementB.Attributes[i].Value))
			{
				return false;
			}
		}
		return true;
	}

	public static ConfigurationElement GetSiteConfiguration(string siteName, ServerManager serverManager)
	{
		return FindElement(serverManager.GetApplicationHostConfiguration().GetSection("system.applicationHost/sites").GetCollection(), ElementTag.Site, AttributeName.Name, siteName);
	}

	public static IEnumerable<ConfigurationElement> FindElements(IEnumerable<ConfigurationElement> collection, string elementTagName, params string[] keyValues)
	{
		return FindElements(collection, elementTagName, singleElement: false, keyValues);
	}

	public static ConfigurationElement FindElement(IEnumerable<ConfigurationElement> collection, string elementTagName, params string[] keyValues)
	{
		return FindElements(collection, elementTagName, singleElement: true, keyValues).FirstOrDefault();
	}

	public static IEnumerable<ConfigurationElement> FindElements(IEnumerable<ConfigurationElement> collection, string elementTagName, bool singleElement, params string[] keyValues)
	{
		List<ConfigurationElement> list = new List<ConfigurationElement>();
		foreach (ConfigurationElement item in collection)
		{
			if (!string.Equals(item.ElementTagName, elementTagName, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			bool flag = true;
			for (int i = 0; i < keyValues.Length; i += 2)
			{
				object attributeValue = item.GetAttributeValue(keyValues[i]);
				string a = null;
				if (attributeValue != null)
				{
					a = attributeValue.ToString();
				}
				if (!string.Equals(a, keyValues[i + 1], StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(item);
				if (singleElement)
				{
					return list;
				}
			}
		}
		return list;
	}

	public static Site GetSite(string websiteIdString, ServerManager iisManager)
	{
		return GetSite(long.Parse(websiteIdString, NumberStyles.None, CultureInfo.InvariantCulture), iisManager);
	}

	public static Site GetSite(long websiteId, ServerManager iisManager)
	{
		return iisManager.Sites.Single((Site x) => x.Id == websiteId);
	}
}
