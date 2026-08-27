using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMonitorPreset")]
[EntityName("ConfigAPIMonitorPresetFolder")]
public class MonitorPresetConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] monitorPresetIds = MonitorPreset.QueryMonitorPresetIdsByMonitorId(requestContext, path.Id);
			MonitorPreset[] array = MonitorPreset.QueryMonitorPresetsByIds(requestContext, monitorPresetIds);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			MonitorPreset[] array2 = array;
			foreach (MonitorPreset preset in array2)
			{
				list.Add(ConvertToMonitorPresetConfigurationItem(path, preset));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("MonitorPresetFolder", ConfigurationItemPath.CreateItemPath("Monitor", path.Id));
		}
		try
		{
			MonitorPreset monitorPreset = MonitorPreset.QueryMonitorPreset(requestContext, path.Id);
			if (monitorPreset == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToMonitorPresetConfigurationItem(ConfigurationItemPath.CreateFolderPath("Monitor", monitorPreset.MonitorId, "MonitorPresetFolder"), monitorPreset);
		}
		catch (MonitorNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
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
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(item, path, out var definitionXml);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			MonitorPreset monitorPreset = MonitorPreset.QueryMonitorPreset(requestContext, path.Id);
			monitorPreset = MonitorPreset.UpdateMonitorPreset(requestContext, path.Id, definitionXml, monitorPreset.CustomProperties);
			ConfigurationItem resultItem = ConvertToMonitorPresetConfigurationItem(parentPath, monitorPreset);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (DeviceUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (MonitorPreset.QueryMonitorPreset(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string definitionXml;
			return ValidateItemInternal(item, path, out definitionXml);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path, out string definitionXml)
	{
		definitionXml = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DefinitionXml", out definitionXml);
				}
				if (string.IsNullOrEmpty(definitionXml))
				{
					throw new ArgumentNullException("DefinitionXml", VmoResources.VMO62002);
				}
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
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

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Monitor", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToMonitorPresetConfigurationItem(ConfigurationItemPath parentPath, MonitorPreset preset)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "MonitorPreset",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = preset.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, preset.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(preset.Id),
			GeneralPropertyDefinitions.GetNameProperty(preset.Name, isSettable: false),
			GeneralPropertyDefinitions.GetLastModifiedProperty(preset.LastModified),
			MonitorPresetPropertyDefinition.MonitorPresetDefinitionXmlProperty(preset.DefinitionXml)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}
}
