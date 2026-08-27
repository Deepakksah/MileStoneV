using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.ConfigurationApi.Validators;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public class ConfigurationApiService : ConfigurationApiBase, IConfigurationApiSupport
{
	private readonly IConfigurationItemBuilder _itemBuilder;

	private readonly IMethodProvider _systemMethodProvider;

	private readonly IMethodProvider _acUnitMethodProvider;

	private readonly ICommonValidator _commonValidator;

	private readonly IMIPServerGroupProxy _mipServerGroupProxy;

	private readonly IMIPObjectProxy _mipObjectProxy;

	private const string AllSystemsGroupId = "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD";

	public ConfigurationApiService(IConfigurationItemBuilder itemBuilder, IMethodProvider systemMethodProvider, IMethodProvider acUnitMethodProvider, ICommonValidator commonValidator, IMIPServerGroupProxy mipServerGroupProxy, IMIPObjectProxy mipObjectProxy)
	{
		_itemBuilder = itemBuilder;
		_systemMethodProvider = systemMethodProvider;
		_acUnitMethodProvider = acUnitMethodProvider;
		_commonValidator = commonValidator;
		_mipServerGroupProxy = mipServerGroupProxy;
		_mipObjectProxy = mipObjectProxy;
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (path.IsFolder)
		{
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			string itemType = path.ItemType;
			if (!(itemType == "AccessControlSystemFolder"))
			{
				if (itemType == "AccessControlUnitFolder")
				{
					if (!GroupMemberUtility.HasPermission(requestContext, path.Id))
					{
						throw new PathNotFoundException(path.Id);
					}
					if (path.ParentItemType == "AccessControlGroup")
					{
						List<Guid> list2 = GroupMemberUtility.QueryMembersInGroup(requestContext, path.Id);
						List<string> list3 = new List<string>();
						foreach (Guid item in list2)
						{
							if (GroupMemberUtility.HasPermission(requestContext, item.ToString()))
							{
								list3.Add(item.ToString());
							}
						}
						MIPObject[] array = MIPObject.QueryMIPObjectsByIds(requestContext, list3.ToArray(), needAdminSecurity: false);
						foreach (MIPObject mIPObject in array)
						{
							ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("AccessControlUnit", mIPObject.Id);
							list.Add(_itemBuilder.BuildAccessControlItem(path2, path, mIPObject.Name, mIPObject, new string[1] { "ChangeSecurityPermissions" }));
						}
					}
					else
					{
						string parentGroupId = _mipObjectProxy.QueryMIPObject(requestContext, path.Id).RelationIds.Split(';').FirstOrDefault().Trim();
						MIPServerGroup[] array2 = _mipServerGroupProxy.QueryServerGroupsByParentGroupId(requestContext, parentGroupId);
						foreach (MIPServerGroup mIPServerGroup in array2)
						{
							MIPObject[] source = _mipObjectProxy.QueryMIPObjects(requestContext, mIPServerGroup.Id);
							if (source.Any())
							{
								string id = source.First().Id;
								if (GroupMemberUtility.HasPermission(requestContext, id))
								{
									ConfigurationItemPath path3 = ConfigurationItemPath.CreateItemPath("AccessControlUnit", id);
									string[] methodIds = new string[0];
									list.Add(_itemBuilder.BuildAccessControlItem(path3, path, mIPServerGroup, source.First(), methodIds));
								}
							}
						}
					}
				}
			}
			else
			{
				MIPServerGroup[] array2 = _mipServerGroupProxy.QueryServerGroupsByParentGroupId(requestContext, "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD");
				foreach (MIPServerGroup mIPServerGroup2 in array2)
				{
					MIPObject[] source2 = _mipObjectProxy.QueryMIPObjects(requestContext, mIPServerGroup2.Id);
					if (source2.Any())
					{
						string id2 = source2.First().Id;
						if (GroupMemberUtility.HasPermission(requestContext, id2))
						{
							ConfigurationItemPath path4 = ConfigurationItemPath.CreateItemPath("AccessControlSystem", source2.First().Id);
							list.Add(_itemBuilder.BuildAccessControlItem(path4, path, mIPServerGroup2, source2.First(), _systemMethodProvider.GetIds()));
						}
					}
				}
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("AccessControlUnitFolder", path);
		if (path.Id.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
		{
			configurationItem.MethodIds = new string[0];
		}
		return new ConfigurationItem[1] { configurationItem };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		PrepareRequest(requestContext, path);
		if (path.IsFolder)
		{
			string itemType = path.ItemType;
			if (itemType == "AccessControlSystemFolder")
			{
				return CreateAccessControlSystemFolderItem(ConfigurationItemPath.CreateSystemPath());
			}
			if (itemType == "AccessControlUnitFolder")
			{
				if (!GroupMemberUtility.HasPermission(requestContext, path.Id))
				{
					throw new PathNotFoundException(path.Id);
				}
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
				ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("AccessControlUnitFolder", configurationItemPath);
				if (configurationItemPath.ItemType == "AccessControlGroup" && !configurationItemPath.Id.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
				{
					configurationItem.MethodIds = new string[2] { "AddAccessControlGroupMember", "RemoveAccessControlGroupMember" };
				}
				return configurationItem;
			}
		}
		else
		{
			if (!GroupMemberUtility.HasPermission(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Id);
			}
			string itemType = path.ItemType;
			if (itemType == "AccessControlSystem")
			{
				try
				{
					MIPObject mIPObject = _mipObjectProxy.QueryMIPObject(requestContext, path.Id);
					string serverGroupId = mIPObject.RelationIds.Split(';').FirstOrDefault().Trim();
					MIPServerGroup serverGroup = _mipServerGroupProxy.QueryServerGroupById(requestContext, serverGroupId);
					ConfigurationItemPath parentPath = ConfigurationItemPath.CreateSystemFolderPath("AccessControlSystemFolder");
					return _itemBuilder.BuildAccessControlItem(path, parentPath, serverGroup, mIPObject, _systemMethodProvider.GetIds());
				}
				catch (MIPObjectNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
				catch (DatabaseConfigExtraRowsException)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			if (itemType == "AccessControlUnit")
			{
				return CreateAccessControlUnitItem(requestContext, path);
			}
		}
		throw new PathNotFoundException(path.Text);
	}

	private ConfigurationItem CreateAccessControlUnitItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		try
		{
			MIPObject mIPObject = _mipObjectProxy.QueryMIPObject(requestContext, path.Id);
			string serverGroupId = mIPObject.RelationIds.Split(';').FirstOrDefault().Trim();
			MIPServerGroup mIPServerGroup = _mipServerGroupProxy.QueryServerGroupById(requestContext, serverGroupId);
			MIPObject mIPObject2 = _mipObjectProxy.QueryMIPObjects(requestContext, mIPServerGroup.ParentId).First();
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath(string.Equals(_mipServerGroupProxy.QueryServerGroupById(requestContext, mIPServerGroup.ParentId).ParentId, "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD", StringComparison.OrdinalIgnoreCase) ? "AccessControlSystem" : "AccessControlUnit", mIPObject2.Id), "AccessControlUnitFolder");
			return _itemBuilder.BuildAccessControlItem(path, parentPath, mIPServerGroup, mIPObject, new string[1] { "ChangeSecurityPermissions" });
		}
		catch (MIPObjectNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DatabaseConfigExtraRowsException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public static ConfigurationItem CreateAccessControlSystemFolderItem(ConfigurationItemPath parentPath)
	{
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty("RuleGroup", ConfigurationApiResources.PropertyAccessControlRuleGroup, string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", "AccessControlRuleGroup", "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD"), new string[1] { "AccessControlRuleGroup" }, "PropertyAccessControlRuleGroup", isSettable: false);
		return GroupConfigurationItemFactory.CreateFolderItem("AccessControlSystemFolder", parentPath, new Property[1] { pathProperty });
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		item.NullCheck("", "InvokeMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\ConfigurationApiService.cs", 237);
		PrepareRequest(requestContext, path);
		if (path.ItemType == "AccessControlSystem")
		{
			return _systemMethodProvider.Provide(item, method).Invoke(requestContext, item, path, parentPath);
		}
		if (path.ItemType == "AccessControlUnitFolder" && parentPath.ItemType == "AccessControlGroup")
		{
			if (path.Id.Equals("9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", StringComparison.OrdinalIgnoreCase))
			{
				throw new PathNotFoundException(string.Format(CultureInfo.InvariantCulture, "Cannot invoke method {0} on the Un-Grouped Folder.", method));
			}
			if (string.Equals(method, "AddAccessControlGroupMember", StringComparison.OrdinalIgnoreCase))
			{
				return _acUnitMethodProvider.Provide(item, method).Invoke(requestContext, item, path, parentPath);
			}
			if (string.Equals(method, "RemoveAccessControlGroupMember", StringComparison.OrdinalIgnoreCase))
			{
				return ((IMethod2)_acUnitMethodProvider.Provide(item, method)).Invoke2(requestContext, item, path, parentPath, this);
			}
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	private void PrepareRequest(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "PrepareRequest", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\AccessControl\\ConfigurationApiService.cs", 274);
		_commonValidator.ValidatePath(path);
	}
}
