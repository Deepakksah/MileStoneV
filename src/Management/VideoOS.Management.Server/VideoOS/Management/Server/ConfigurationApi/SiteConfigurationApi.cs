using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPISite")]
[EntityName("ConfigAPISiteFolder")]
[EntityName("ConfigAPISiteAddressFolder")]
public class SiteConfigurationApi : SiteConfigurationApiObjectBase
{
	public SiteConfigurationApi(ISiteRepository repository)
		: base(repository)
	{
	}

	public SiteConfigurationApi()
		: this(null)
	{
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		string id = path.Id;
		if (path.IsFolder)
		{
			string id2 = path.Id;
			if (string.IsNullOrEmpty(path.Id))
			{
				id2 = base.Repository.GetServerInformation(requestContext).Id;
			}
			if (path.ItemType == "SiteFolder")
			{
				Site[] array = base.Repository.QuerySlaveSites(requestContext, id2);
				List<ConfigurationItem> list = new List<ConfigurationItem>();
				Site[] array2 = array;
				foreach (Site site in array2)
				{
					list.Add(ConvertToSiteInformation(path, site));
				}
				return list.ToArray();
			}
			if (path.ItemType == "SiteAddressFolder")
			{
				Site site2 = base.Repository.QuerySite(requestContext, id2);
				List<ConfigurationItem> list2 = new List<ConfigurationItem>();
				int num = 0;
				ServerAddress[] addresses = site2.Addresses;
				foreach (ServerAddress addresses2 in addresses)
				{
					num++;
					ConfigurationItem item = ConvertToConfigurationItem(path, addresses2, num);
					list2.Add(item);
				}
				return list2.ToArray();
			}
			throw new PathNotFoundException(path.Text);
		}
		try
		{
			base.Repository.QuerySite(requestContext, id);
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ItemType, id);
			return new ConfigurationItem[2]
			{
				GroupConfigurationItemFactory.CreateFolderItem("SiteFolder", parentPath),
				GroupConfigurationItemFactory.CreateFolderItem("SiteAddressFolder", parentPath)
			};
		}
		catch (Exception)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		string id = path.Id;
		Site site;
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, ConfigurationItemPath.CreateSystemPath());
			}
			try
			{
				site = base.Repository.QuerySite(requestContext, id);
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, id);
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
			}
			catch (Exception)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		try
		{
			site = base.Repository.QuerySite(requestContext, path.Id);
		}
		catch (Exception)
		{
			site = null;
		}
		if (site == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateSystemFolderPath("SiteFolder");
		if (!string.IsNullOrEmpty(site.MasterSiteId) && site.MasterSiteId != base.Repository.QueryHomeSite(requestContext).Id)
		{
			parentPath2 = ConfigurationItemPath.CreateFolderPath("Site", site.MasterSiteId, "SiteFolder");
		}
		return ConvertToSiteInformation(parentPath2, site);
	}

	public override ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (methodId == "ChangeSecurityPermissions")
		{
			return InvokeSecurityMethod(requestContext, item, path, methodId);
		}
		throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
	}

	private static ConfigurationItem ConvertToSiteInformation(ConfigurationItemPath parentPath, Site site)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("Site", site.Id);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "Site";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = site.Name;
		configurationItem.ParentPath = ((parentPath != null) ? parentPath.Text : "");
		configurationItem.Path = configurationItemPath.Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(site.Id));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(site.Description, isSettable: false));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(site.LastModified));
		list.Add(SitePropertyDefinition.ConnectionState(site.ConnectionState));
		list.Add(ManagementServerPropertyDefinition.VersionProperty(new Version(site.Version)));
		list.Add(ManagementServerPropertyDefinition.ServiceAccountProperty(site.ServiceAccount));
		list.Add(ManagementServerPropertyDefinition.SynchronizationStatusProperty(site.SynchronizationStatus));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[1] { "ChangeSecurityPermissions" };
		configurationItem.ChildrenFilled = true;
		ConfigurationItem[] children = new ConfigurationItem[2]
		{
			GroupConfigurationItemFactory.CreateFolderItem("SiteFolder", configurationItemPath),
			GroupConfigurationItemFactory.CreateFolderItem("SiteAddressFolder", configurationItemPath)
		};
		configurationItem.Children = children;
		return configurationItem;
	}

	private static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, ServerAddress addresses, int index)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder("SiteAddress", index.ToString(CultureInfo.InvariantCulture)).CreatePath();
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "SiteAddress",
			UIHint = "DirectionHorizontal"
		};
		List<Property> list = new List<Property>
		{
			ManagementServerPropertyDefinition.ServerAddressUrlProperty(addresses.URL),
			ManagementServerPropertyDefinition.ServerAddressTypeProperty(addresses.Type)
		};
		obj.DisplayName = addresses.URL;
		obj.Properties = list.ToArray();
		obj.Path = configurationItemPath.Text;
		obj.ParentPath = parentPath.Text;
		return obj;
	}

	public static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!(path.ParentItemType == "System"))
			{
				if (!string.Equals(path.ParentItemType, "Site", StringComparison.Ordinal))
				{
					throw new PathInvalidException("Invalid parent. Path: " + path.Text);
				}
				if (!path.Id.IsGuid())
				{
					throw new PathInvalidException("Invalid id. Path: " + path.Text);
				}
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}
}
