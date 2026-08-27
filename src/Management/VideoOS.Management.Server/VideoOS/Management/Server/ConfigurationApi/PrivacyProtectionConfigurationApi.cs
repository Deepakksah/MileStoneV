using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIPrivacyProtection")]
[EntityName("ConfigAPIPrivacyProtectionFolder")]
public class PrivacyProtectionConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string PrivacyMaskXml = "PrivacyMaskXml";

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
		string itemType = path.ItemType;
		string id = path.Id;
		DevicePrivacyProtection devicePrivacyProtection = DevicePrivacyProtection.QueryPrivacyProtectionById(requestContext, id);
		if (devicePrivacyProtection == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (string.Equals(itemType, "PrivacyProtectionFolder", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[1] { ConvertToPrivacyProtectionConfigurationItem(path, devicePrivacyProtection) };
		}
		if (string.Equals(itemType, "PrivacyProtection", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
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
		if (string.Equals(path.ItemType, "PrivacyProtectionFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("PrivacyProtectionFolder", parentPath);
		}
		string id = path.Id;
		DevicePrivacyProtection devicePrivacyProtection = DevicePrivacyProtection.QueryPrivacyProtectionById(requestContext, id);
		if (devicePrivacyProtection == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToPrivacyProtectionConfigurationItem(ConfigurationItemPath.CreateFolderPath("Camera", devicePrivacyProtection.Id, "PrivacyProtectionFolder"), devicePrivacyProtection);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!string.Equals(path.ItemType, "PrivacyProtection", StringComparison.Ordinal))
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var enabled, out var privacyMaskXml);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			DevicePrivacyProtection privacyProtection = DevicePrivacyProtection.UpdatePrivacyProtection(requestContext, path.Id, enabled, privacyMaskXml);
			ConfigurationItem resultItem = ConvertToPrivacyProtectionConfigurationItem(parentPath, privacyProtection);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "PrivacyProtection", StringComparison.Ordinal))
		{
			if (DevicePrivacyProtection.QueryPrivacyProtectionById(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			bool? enabled;
			string privacyMaskXml;
			return ValidateItemInternal(requestContext, item, path, out enabled, out privacyMaskXml);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out bool? enabled, out string privacyMaskXml)
	{
		if (string.Equals(path.ItemType, "PrivacyProtection", StringComparison.Ordinal))
		{
			privacyMaskXml = null;
			enabled = null;
			try
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PrivacyMaskXml", out privacyMaskXml);
				}
				if (item.EnableProperty != null)
				{
					enabled = item.EnableProperty.Enabled;
				}
			}
			catch (ArgumentException exception)
			{
				ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			try
			{
				DevicePrivacyProtection.UpdatePrivacyProtectionValidation(requestContext, path.Id, privacyMaskXml);
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			catch (ArgumentNullException exception2)
			{
				ValidateResult validateResult2 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
				if (validateResult2 != null)
				{
					return validateResult2;
				}
				throw;
			}
			catch (ArgumentException exception3)
			{
				ValidateResult validateResult3 = ErrorHelper.CreateValidateResult(exception3, item.Properties);
				if (validateResult3 != null)
				{
					return validateResult3;
				}
				throw;
			}
		}
		throw new NotSupportedException();
	}

	private static ConfigurationItem ConvertToPrivacyProtectionConfigurationItem(ConfigurationItemPath parentPath, DevicePrivacyProtection privacyProtection)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "PrivacyProtection";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypePrivacyProtectionFolder;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, privacyProtection.Id).Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(privacyProtection.Enabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(privacyProtection.Id));
		list.Add(PrivacyProtectionPropertyDefinition.PrivacyMaskXml(privacyProtection.PrivacyMaskXml));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Camera", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid id. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}
}
