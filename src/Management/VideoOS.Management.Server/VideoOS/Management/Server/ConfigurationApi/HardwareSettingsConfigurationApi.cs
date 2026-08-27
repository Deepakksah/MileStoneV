using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIHardwareDriverSettings")]
[EntityName("ConfigAPIHardwareDriverSettingsFolder")]
public class HardwareSettingsConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (!Hardware.HardwareExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			ValidateResult validateResult = SaveItemInternal(requestContext, item, path);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = GetItem(requestContext, path);
			return validateResult;
		}
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (!Hardware.HardwareExists(requestContext, path.Id))
			{
				throw new PathNotFoundException(path.Text);
			}
			return ValidateItemInternal(requestContext, item, path);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (!path.IsFolder)
			{
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DriverSettings hardwareSettings = null;
				DriverSettingsBusiness.QueryHardwareSettings(requestContext, path.Id, out mainDefinition, out hardwareSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(item, mainDefinition, out updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
				list = ValidateUpdatedSettings(requestContext, path.Id, updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static ValidateResult SaveItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (!path.IsFolder)
			{
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				DriverSettings updatedDriverSettings = new DriverSettings();
				MainDefinition mainDefinition = null;
				DriverSettings hardwareSettings = null;
				DriverSettingsBusiness.QueryHardwareSettings(requestContext, path.Id, out mainDefinition, out hardwareSettings);
				List<ErrorResult> list = SettingsUtil.ConvertToDriverSettings(item, mainDefinition, out updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
				list = SaveUpdatedSettings(requestContext, path.Id, updatedDriverSettings);
				if (list.Count > 0)
				{
					validateResult.ErrorResults = list.ToArray();
					validateResult.ValidatedOk = false;
				}
				return validateResult;
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static List<ErrorResult> ValidateUpdatedSettings(IGeminiRequestContext requestContext, string id, DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		try
		{
			DriverSettingsBusiness.ValidateHardwareSettings(requestContext, id, updatedDriverSettings);
		}
		catch (UpdateDriverSettingsException e)
		{
			list.Add(SettingsUtil.ConvertException(e));
		}
		catch (InvalidDefinitionException)
		{
			list.Add(SettingsUtil.ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition"));
		}
		return list;
	}

	private static List<ErrorResult> SaveUpdatedSettings(IGeminiRequestContext requestContext, string hardwareId, DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		try
		{
			DriverSettingsBusiness.UpdateHardwareSettings(requestContext, hardwareId, updatedDriverSettings);
		}
		catch (UpdateDriverSettingsException e)
		{
			list.Add(SettingsUtil.ConvertException(e));
		}
		catch (InvalidDefinitionException)
		{
			list.Add(SettingsUtil.ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition"));
		}
		return list;
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
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			MainDefinition mainDefinition = null;
			HardwareHwid hwid = new HardwareHwid("hardware");
			DriverSettings hardwareSettings = null;
			DriverSettingsBusiness.QueryHardwareSettings(requestContext, path.Id, out mainDefinition, out hardwareSettings);
			ConfigurationItem configurationItem = SettingsUtil.ConstructDriverSettingsItemAndChildren(requestContext, mainDefinition, hwid, "HardwareDriverSettings", hardwareSettings, path);
			return new ConfigurationItem[1] { configurationItem };
		}
		return new ConfigurationItem[0];
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
			string id = path.Id;
			Hardware.QueryHardwareById(requestContext, id);
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("Hardware", id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath("Hardware", path.Id, "HardwareDriverSettingsFolder");
		MainDefinition mainDefinition = null;
		HardwareHwid hwid = new HardwareHwid("hardware");
		DriverSettings hardwareSettings = null;
		DriverSettingsBusiness.QueryHardwareSettings(requestContext, path.Id, out mainDefinition, out hardwareSettings);
		return SettingsUtil.ConstructDriverSettingsItemAndChildren(requestContext, mainDefinition, hwid, "HardwareDriverSettings", hardwareSettings, parentPath2);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals("HardwareDriverSettingsFolder", path.ItemType, StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
		else
		{
			if (!string.Equals("HardwareDriverSettings", path.ItemType, StringComparison.Ordinal) && !string.Equals("Hardware", path.ItemType, StringComparison.Ordinal))
			{
				throw new PathInvalidException("Unsupported item type. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
	}
}
