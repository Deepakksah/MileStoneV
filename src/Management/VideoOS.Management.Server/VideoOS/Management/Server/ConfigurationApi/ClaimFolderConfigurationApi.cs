using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIClaimFolder")]
public class ClaimFolderConfigurationApi : ClaimConfigurationApiObjectBase
{
	public ClaimFolderConfigurationApi()
		: this(null)
	{
	}

	public ClaimFolderConfigurationApi(IClaimRepository repository)
		: base(repository)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
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
		Claim[] array = Array.Empty<Claim>();
		if (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			string id = path.Id;
			array = base.Repository.QueryRoleClaims(requestContext, id);
		}
		else
		{
			if (!string.Equals(path.ParentItemType, "BasicUser", StringComparison.Ordinal))
			{
				throw new NotSupportedException();
			}
			array = BasicUser.QueryBasicUserById(requestContext, path.Id)?.Claims.ToArray() ?? Array.Empty<Claim>();
		}
		ConfigurationItem[] array2 = new ConfigurationItem[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = ClaimConfigurationApiObjectBase.ConvertToConfigurationItem(array[i], path);
		}
		return array2;
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
		if (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal) && string.Equals(path.ItemType, "ClaimFolder", StringComparison.Ordinal))
		{
			string id = path.Id;
			if (!Role.RoleExists(requestContext, id))
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder
			{
				ItemType = "Role",
				Id = id
			}.CreatePath();
			return GroupConfigurationItemFactory.CreateFolderItem("ClaimFolder", parentPath);
		}
		if (string.Equals(path.ParentItemType, "BasicUser", StringComparison.Ordinal) && string.Equals(path.ItemType, "ClaimFolder", StringComparison.Ordinal))
		{
			if (BasicUser.QueryBasicUserById(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath2 = new ConfigurationItemPathBuilder
			{
				ItemType = "BasicUser",
				Id = path.Id
			}.CreatePath();
			return GroupConfigurationItemFactory.CreateFolderItem("ClaimFolder", parentPath2);
		}
		throw new NotSupportedException();
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		if (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			if (methodId == "AddRoleClaim")
			{
				return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodAddRoleClaim, item, ClaimPropertyDefinition.LoginProviderIdProperty("", isSettable: true), ClaimPropertyDefinition.ClaimNameProperty("", isSettable: true), ClaimPropertyDefinition.ClaimValueProperty("", isSettable: true));
			}
			if (methodId == "RemoveRoleClaim")
			{
				return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodRemoveRoleClaim, item, ClaimPropertyDefinition.LoginProviderIdProperty("", isSettable: true), ClaimPropertyDefinition.ClaimNameProperty("", isSettable: true), ClaimPropertyDefinition.ClaimValueProperty("", isSettable: true));
			}
		}
		throw new ArgumentException("Unknown methodId: " + methodId, "methodId");
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			if (methodId == "AddRoleClaim")
			{
				return AddRoleClaim(requestContext, item, path);
			}
			if (methodId == "RemoveRoleClaim")
			{
				return RemoveRoleClaim(requestContext, item, path);
			}
		}
		throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfFolderPathInvalid(path);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.ParentItemType != null && path.ParentItemType.Length == 0 && !string.IsNullOrEmpty(path.Id) && (string.Equals(path.ItemType, "Role", StringComparison.Ordinal) || string.Equals(path.ItemType, "BasicUser", StringComparison.Ordinal)))
		{
			return;
		}
		throw new PathInvalidException("Invalid claim folder path: " + path?.Text);
	}

	private static void ThrowIfFolderPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.IsFolder && (string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal) || string.Equals(path.ParentItemType, "BasicUser", StringComparison.Ordinal)))
		{
			return;
		}
		throw new PathInvalidException("Invalid claim folder path: " + path?.Text);
	}

	private ConfigurationItem AddRoleClaim(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		string text = null;
		string value = null;
		string value2 = null;
		string value3 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimProvider", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimName", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimValue", out value3);
		}
		if (!string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		text = path.Id;
		if (!Role.RoleExists(requestContext, text))
		{
			throw new PathNotFoundException(path.Text);
		}
		try
		{
			return MethodTypesHelper.CreateInvokeResult(ClaimConfigurationApiObjectBase.ConvertToConfigurationItem(base.Repository.AddClaimToRole(requestContext, text, value, value2, value3), path), null, "Result", "Success", true);
		}
		catch (VmoServerException)
		{
			throw;
		}
	}

	private ConfigurationItem RemoveRoleClaim(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		string text = null;
		string value = null;
		string value2 = null;
		string value3 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimProvider", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimName", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ClaimValue", out value3);
		}
		if (!string.Equals(path.ParentItemType, "Role", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		text = path.Id;
		if (!Role.RoleExists(requestContext, text))
		{
			throw new PathNotFoundException(path.Text);
		}
		try
		{
			base.Repository.RemoveClaimFromRole(requestContext, text, value, value2, value3);
			return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
		}
		catch (VmoServerException)
		{
			throw;
		}
	}
}
