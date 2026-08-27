using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl.Methods;

public class AddGroupMember : IMethod
{
	private IMIPObjectProxy _mipObjectProxy;

	public string Id => "AddAccessControlGroupMember";

	public AddGroupMember(IMIPObjectProxy mipObjectProxy)
	{
		_mipObjectProxy = mipObjectProxy;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		Property pathSelectionProperty = GeneralPropertyDefinitions.GetPathSelectionProperty("AccessControlUnit", "ItemSelection", ConfigurationApiResources.MethodAddAccessControlGroupMember);
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodAddAccessControlGroupMember, item, pathSelectionProperty);
	}

	private ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationItemPath configurationItemPath = FetchAndValidateSelectedPath(item);
		MIPObject mIPObject = _mipObjectProxy.QueryMIPObject(requestContext, configurationItemPath.Id);
		if (mIPObject == null || mIPObject.Kind != "6d3bdb33-d913-470f-8784-3dd4ee94db8c")
		{
			throw new ArgumentException("Selected item not found, or is not an Access Control object");
		}
		List<Guid> obj = new List<Guid>
		{
			new Guid(mIPObject.Id)
		};
		List<Guid> list = GroupMemberUtility.QueryMembersInGroup(requestContext, "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF");
		List<Guid> list2 = new List<Guid>();
		foreach (Guid item2 in obj)
		{
			if (list.Contains(item2))
			{
				list2.Add(item2);
			}
		}
		GroupMemberUtility.RemoveMembers(requestContext, _mipObjectProxy, "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", configurationItemPath.Id, list2);
		GroupMemberUtility.AddMembers(requestContext, path.Id, configurationItemPath.Id, list2);
		return MethodTypesHelper.CreateInvokeResult(item, (string)null, "Result", "Success", true, (Property[])null);
	}

	private static ConfigurationItemPath FetchAndValidateSelectedPath(ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out string value) || string.IsNullOrEmpty(value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or value not specified: {0}", "ItemSelection"));
		}
		try
		{
			return ConfigurationItemPath.CreatePath(value);
		}
		catch (ArgumentException inner)
		{
			throw new PropertyInvalidException("Invalid path. " + value, "ItemSelection", inner);
		}
	}
}
