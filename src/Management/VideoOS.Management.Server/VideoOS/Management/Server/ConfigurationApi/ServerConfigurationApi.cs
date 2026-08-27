using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Services;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIServer")]
[EntityName("ConfigAPIServerFolder")]
public class ServerConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private HashSet<Guid> SupportedServers = new HashSet<Guid>
	{
		ServiceTypes.ServiceType_LogService,
		ServiceTypes.ServiceType_EventServerService,
		ServiceTypes.ServiceType_MobileService
	};

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		if (!path.IsFolder)
		{
			return Array.Empty<ConfigurationItem>();
		}
		return (from s in RegisteredServer.QueryAllRegisteredServers(requestContext, includeDisabled: false)
			where SupportedServers.Contains(s.Type)
			select ConvertToConfigurationItem(path, s, "Server")).ToArray();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
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
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("ServerFolder", ConfigurationItemPath.CreateSystemPath());
		}
		RegisteredServer registeredServer = RegisteredServer.QueryRegisteredServer(requestContext, new Guid(path.Id));
		if (registeredServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "ServerFolder").CreatePath(), registeredServer, path.ItemType);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	private static ConfigurationItem ConvertToConfigurationItem(ConfigurationItemPath parentPath, RegisteredServer registeredServer, string itemType)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = itemType,
			ItemCategory = ItemCategories.Item,
			DisplayName = registeredServer.Name,
			UIHint = "",
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, registeredServer.IDServer.ToString()).Text;
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(registeredServer.IDServer),
			GeneralPropertyDefinitions.GetNameProperty(registeredServer.Name, isSettable: false),
			GeneralPropertyDefinitions.GetDescriptionProperty(registeredServer.Description, isSettable: false),
			ServerPropertyDefinitions.AddressesProperty(registeredServer.Addresses.Select((ServerAddress a) => a.URL)),
			ServerPropertyDefinitions.TypeProperty(registeredServer.Type)
		};
		obj.Properties = list.ToArray();
		return obj;
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}
}
