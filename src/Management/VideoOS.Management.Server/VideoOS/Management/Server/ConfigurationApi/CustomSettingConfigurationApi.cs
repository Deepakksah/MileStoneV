using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPICustomSettingGlobal")]
[EntityName("ConfigAPICustomSettingUser")]
public class CustomSettingConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(item, out var xml, out var version);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = xml,
			DataVersion = version + 1
		};
		try
		{
			if (path.ItemType == "CustomSettingGlobal")
			{
				CustomSetting.SetCustomSettingDataGlobal(requestContext, new Guid(path.Id), customSettingInternalData, forceOverride: false);
			}
			else
			{
				CustomSetting.SetCustomSettingDataUser(requestContext, new Guid(path.Id), customSettingInternalData, forceOverride: false);
			}
		}
		catch (CustomSettingOutOfSyncException ex)
		{
			validateResult = ErrorHelper.CreateValidateResult(new PropertyInvalidException(ex.Message, "DataVersion"), item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string xml;
		int version;
		return ValidateItemInternal(item, out xml, out version);
	}

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
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		Guid guid = new Guid(path.Id);
		CustomSetting customSetting = ((!(path.ItemType == "CustomSettingGlobal")) ? CustomSetting.GetCustomSettingDataUser(requestContext, guid) : CustomSetting.GetCustomSettingDataGlobal(requestContext, guid));
		if (customSetting == null)
		{
			customSetting = new CustomSetting
			{
				CustomSettingDataXml = "",
				DataVersion = 0,
				Id = guid,
				LastModifiedTime = DateTime.UtcNow,
				LastModifiedUser = ""
			};
		}
		return ConvertToConfigurationItem(customSetting, path);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, out string xml, out int version)
	{
		xml = null;
		version = 0;
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DataXml", out xml);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DataVersion", out string value);
				if (!int.TryParse(value, out version))
				{
					throw new PropertyInvalidException("Integer expected", "DataVersion");
				}
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
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
	}

	protected static ConfigurationItem ConvertToConfigurationItem(CustomSetting customSetting, ConfigurationItemPath path)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateSystemPath();
		return new ConfigurationItem
		{
			UIHint = "DirectionHorizontal",
			ItemType = path.ItemType,
			ItemCategory = ItemCategories.Item,
			DisplayName = "N/A",
			ParentPath = configurationItemPath.Text,
			Path = path.Text,
			ChildrenFilled = true,
			Properties = new List<Property>
			{
				GeneralPropertyDefinitions.GetIdPropertyGuid(customSetting.Id),
				GeneralPropertyDefinitions.GetNameProperty("N/A"),
				GeneralPropertyDefinitions.GetLastModifiedProperty(customSetting.LastModifiedTime),
				CustomSettingPropertyDefinition.GetCustomSettingDataXmlProperty(customSetting.CustomSettingDataXml),
				CustomSettingPropertyDefinition.GetDataVersionProperty(customSetting.DataVersion),
				CustomSettingPropertyDefinition.GetLastModifiedUserProperty(customSetting.LastModifiedUser)
			}.ToArray()
		};
	}
}
