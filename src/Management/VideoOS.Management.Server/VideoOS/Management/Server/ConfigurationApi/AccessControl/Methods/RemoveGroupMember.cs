using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl.Methods;

public class RemoveGroupMember : IMethod2, IMethod
{
	private IMIPObjectProxy _mipObjectProxy;

	public string Id => "RemoveAccessControlGroupMember";

	public RemoveGroupMember(IMIPObjectProxy mipObjectProxy)
	{
		_mipObjectProxy = mipObjectProxy;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException("Use Invoke2 instead. This method is not supported in this context.");
	}

	public ConfigurationItem Invoke2(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, IConfigurationApiSupport service)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, service.GetChildItems(requestContext, path), Id);
		}
		return InvokeMethod(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (ConfigurationItem configurationItem in members)
		{
			dictionary.Add(configurationItem.Path, configurationItem.DisplayName);
		}
		Property enumProperty = GeneralPropertyDefinitions.GetEnumProperty("ItemSelection", "", ConfigurationApiResources.MethodRemoveAccessControlGroupMember, "MethodRemoveAccessControlGroupMember", null, dictionary);
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodRemoveAccessControlGroupMember, item, enumProperty);
	}

	private ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath folderPath)
	{
		if (string.CompareOrdinal(folderPath.ParentItemType, "AccessControlGroup") != 0 || !folderPath.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid path: " + folderPath.Text);
		}
		string value = null;
		if (!((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ItemSelection", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		if (configurationItemPath.ItemType != "AccessControlUnit")
		{
			throw new PropertyInvalidException("Selected item is not an Access Control Unit.", "ItemSelection");
		}
		MIPObject mIPObject = _mipObjectProxy.QueryMIPObject(requestContext, configurationItemPath.Id);
		List<Guid> memberIds = new List<Guid>
		{
			new Guid(mIPObject.Id)
		};
		GroupMemberUtility.RemoveMembers(requestContext, _mipObjectProxy, folderPath.Id, configurationItemPath.Id, memberIds);
		GroupMemberUtility.AddMembers(requestContext, "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", configurationItemPath.Id, memberIds);
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(configurationItem);
	}
}
