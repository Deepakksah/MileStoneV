using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIPtzPreset")]
[EntityName("ConfigAPIPtzPresetFolder")]
public class PtzPresetConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Pan", out double? value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Tilt", out double? value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Zoom", out double? value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Locked", out bool? value6);
		try
		{
			PtzPreset preset = DevicePtzPresets.UpdatePtzPreset(requestContext, parentPath.Id, path.Id, value, value2, value3, value4, value5, null, value6).First((PtzPreset p) => string.Equals(PropertyTypesHelper.GetValue(p.Id), path.Id, StringComparison.Ordinal));
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = ConvertToPresetConfigurationItem(parentPath, preset);
			return validateResult;
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
		}
		catch (DevicePtzPresetNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DevicePtzPresetsNotSupportedException)
		{
			throw new NotSupportedException("Ptz presets not supported");
		}
		catch (DevicePtzPresetNotUpdatableException)
		{
			throw new NotSupportedException("Device presets can not be changed");
		}
		catch (DevicePtzPresetNotUniqueException)
		{
			return ErrorHelper.ErrorResultNameAlreadyExists();
		}
		catch (ArgumentException exception)
		{
			validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return ValidateItemInternal(requestContext, item, path, parentPath);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
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
			bool? value = false;
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Pan", out double? value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Tilt", out double? value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Zoom", out double? value6);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Locked", out value);
			try
			{
				DevicePtzPresets.ValidatePtzPresetUpdate(requestContext, parentPath.Id, path.Id, value2, value3, value4, value5, value6, null, value);
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			catch (DeviceNotFoundException)
			{
				throw new PathNotFoundException(parentPath.Text);
			}
			catch (DevicePtzPresetNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (DevicePtzPresetsNotSupportedException)
			{
				throw new NotSupportedException("Ptz presets not supported");
			}
			catch (DevicePtzPresetNotUpdatableException)
			{
				throw new NotSupportedException("Device presets can not be changed");
			}
			catch (DevicePtzPresetNotUniqueException)
			{
				return ErrorHelper.ErrorResultNameAlreadyExists();
			}
			catch (ArgumentException ex6)
			{
				ArgumentException ex7 = ex6;
				ArgumentException ex8 = ex7;
				ValidateResult validateResult = ErrorHelper.CreateValidateResult(ex8, item.Properties.Where((Property p) => string.Equals(p.Key.ToUpperInvariant(), ex8.ParamName.ToUpperInvariant(), StringComparison.Ordinal)));
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
		}
		throw new NotSupportedException();
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
			try
			{
				PtzPresetCollection ptzPresetCollection = DevicePtzPresets.QueryPtzPresets(requestContext, path.Id);
				List<ConfigurationItem> list = new List<ConfigurationItem>();
				foreach (PtzPreset item in ptzPresetCollection)
				{
					list.Add(ConvertToPresetConfigurationItem(path, item));
				}
				return list.ToArray();
			}
			catch (DevicePtzPresetsNotSupportedException ex)
			{
				throw new NotSupportedException(ex.Message);
			}
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
			if (!DevicePtzPresets.CanLoadPresetsFromDevice(requestContext, path.Id))
			{
				throw new NotSupportedException();
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("PtzPresetFolder", parentPath);
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		ThrowIfPathInvalid(path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddPtzPreset":
			if (!flag)
			{
				return CreateInvokeInfoAddPtzPreset(item, method);
			}
			return AddPtzPreset(requestContext, item, path);
		case "RemovePtzPreset":
			if (!flag)
			{
				return CreateInvokeInfoRemovePtzPreset(item, GetChildItems(requestContext, path), method);
			}
			return RemovePtzPreset(requestContext, item, path);
		case "DefaultPtzPreset":
			if (!flag)
			{
				return CreateInvokeInfoDefaultPtzPreset(item, GetChildItems(requestContext, path), method);
			}
			return SetDefaultPtzPreset(requestContext, item, path);
		case "GetDevicePresets":
			if (!flag)
			{
				return CreateInvokeInfoItemGetDevicePresets(item, method);
			}
			return GetDevicePresets(requestContext, item, path);
		default:
			throw new ArgumentException("Unknown methodId:" + method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoItemGetDevicePresets(ConfigurationItem item, string methodId)
	{
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.WizardGetDevicePresets, item, GetDevicePresetsTaskResultDefinition.SessionDataIdProperty(0L));
	}

	private static ConfigurationItem GetDevicePresets(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetDevicePresets", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PtzPreset\\PtzPresetConfigurationApi.cs", 303);
		item.NullCheck("", "GetDevicePresets", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PtzPreset\\PtzPresetConfigurationApi.cs", 304);
		path.NullCheck("", "GetDevicePresets", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\PtzPreset\\PtzPresetConfigurationApi.cs", 305);
		string id = path.Id;
		if (!DevicePtzPresets.CanLoadPresetsFromDevice(requestContext, id))
		{
			throw new NotSupportedException("");
		}
		string recordingServerId = Device.QueryDeviceById(requestContext, id).RecordingServerId;
		long result = 0L;
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SessionDataId", out string value) && !long.TryParse(value, out result))
		{
			result = 0L;
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.GetDevicePresets(requestContext, recordingServerId, id, result), item);
	}

	private static ConfigurationItem CreateInvokeInfoAddPtzPreset(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddPtzPreset;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[6]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid()),
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			PtzPresetPropertyDefinition.PanProperty(0.0),
			PtzPresetPropertyDefinition.TiltProperty(0.0),
			PtzPresetPropertyDefinition.ZoomProperty(0.0)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemovePtzPreset(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (ConfigurationItem item2 in members)
		{
			list.Add(item2);
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemovePtzPreset, "PropertyRemovePtzPreset", ConfigurationApiResources.PropertyRemovePtzPreset, item, list.ToArray());
	}

	private static ConfigurationItem CreateInvokeInfoDefaultPtzPreset(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>(members);
		list.Add(new ConfigurationItem
		{
			DisplayName = ConfigurationApiResources.WizardDefaultPtzPresetUnset,
			Path = ""
		});
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardDefaultPtzPreset, "PropertyDefaultPtzPreset", ConfigurationApiResources.PropertyDefaultPtzPreset, item, list.ToArray());
	}

	private static ConfigurationItem AddPtzPreset(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Pan", out double? value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Tilt", out double? value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Zoom", out double? value6);
			if (value == null)
			{
				throw new PropertyInvalidException("Property value missing", "Id");
			}
			if (value2 == null)
			{
				throw new PropertyInvalidException("Property value missing", "Name");
			}
			if (value3 == null)
			{
				throw new PropertyInvalidException("Property value missing", "Description");
			}
			if (!value4.HasValue)
			{
				throw new PropertyInvalidException("Property value missing", "Pan");
			}
			if (!value5.HasValue)
			{
				throw new PropertyInvalidException("Property value missing", "Tilt");
			}
			if (!value6.HasValue)
			{
				throw new PropertyInvalidException("Property value missing", "Zoom");
			}
			try
			{
				DevicePtzPresets.AddPtzPreset(requestContext, path.Id, value, value2, value3, value4.Value, value5.Value, value6.Value, null, locked: false);
			}
			catch (DeviceNotFoundException)
			{
				throw new PathNotFoundException(path.Text);
			}
			catch (DevicePtzPresetsNotSupportedException)
			{
				throw new NotSupportedException("Ptz presets not supported");
			}
			catch (DevicePtzPresetNotUpdatableException)
			{
				throw new NotSupportedException("Device presets can not be changed");
			}
			catch (DevicePtzPresetAlreadyExistsException ex4)
			{
				throw new ArgumentException(ex4.Message);
			}
			catch (DevicePtzPresetNotUniqueException ex5)
			{
				throw new ArgumentException(ex5.Message);
			}
			catch (DeviceNotRecentVersionException ex6)
			{
				throw new ArgumentException(ex6.Message);
			}
			return MethodTypesHelper.CreateInvokeResult(item, ConfigurationItemPath.CreateItemPath("PtzPreset", value).Text, "Result", "Success", true);
		}
		throw new PropertyInvalidException("Item properties missing");
	}

	private static ConfigurationItem RemovePtzPreset(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			DevicePtzPresets.RemovePtzPreset(requestContext, path.Id, invokeMemberItemSelectionPath.Id);
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DevicePtzPresetNotFoundException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		catch (DevicePtzPresetsNotSupportedException)
		{
			throw new NotSupportedException("Ptz presets not supported");
		}
		catch (DevicePtzPresetNotUpdatableException)
		{
			throw new NotSupportedException("Device presets can not be changed");
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem SetDefaultPtzPreset(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		try
		{
			if (item.Properties.Where((Property p) => p.Key == "ItemSelection" && string.IsNullOrWhiteSpace(p.Value)).Any())
			{
				PtzPreset ptzPreset = (from p in DevicePtzPresets.QueryPtzPresets(requestContext, path.Id)
					where p.IsDefault
					select p).FirstOrDefault();
				if (ptzPreset != null)
				{
					DevicePtzPresets.UpdatePtzPreset(requestContext, path.Id, PropertyTypesHelper.GetValue(ptzPreset.Id), null, null, null, null, null, false, null);
				}
			}
			else
			{
				ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
				try
				{
					DevicePtzPresets.UpdatePtzPreset(requestContext, path.Id, invokeMemberItemSelectionPath.Id, null, null, null, null, null, true, null);
				}
				catch (DevicePtzPresetNotFoundException)
				{
					throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
				}
				catch (DevicePtzPresetNotUpdatableException)
				{
					throw new NotSupportedException(invokeMemberItemSelectionPath.Text);
				}
			}
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (DevicePtzPresetsNotSupportedException)
		{
			throw new NotSupportedException("Ptz presets not supported");
		}
		catch (DevicePtzPresetNotUniqueException ex5)
		{
			throw new ArgumentException(ex5.Message);
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "Camera", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToPresetConfigurationItem(ConfigurationItemPath parentPath, PtzPreset preset)
	{
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(preset.Id),
			GeneralPropertyDefinitions.GetNameProperty(preset.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(preset.Description),
			PtzPresetPropertyDefinition.DevicePresetProperty(preset.IsDevicePreset),
			PtzPresetPropertyDefinition.DevicePresetInternalIdProperty(preset.IsDevicePreset ? preset.Key : string.Empty),
			PtzPresetPropertyDefinition.DefaultPresetProperty(preset.IsDefault),
			PtzPresetPropertyDefinition.LockedProperty(preset.Locked)
		};
		if (!preset.IsDevicePreset)
		{
			list.Add(PtzPresetPropertyDefinition.PanProperty(preset.PtzArgs.Pan));
			list.Add(PtzPresetPropertyDefinition.TiltProperty(preset.PtzArgs.Tilt));
			list.Add(PtzPresetPropertyDefinition.ZoomProperty(preset.PtzArgs.Zoom));
		}
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "PtzPreset";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = preset.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath("PtzPreset", PropertyTypesHelper.GetValue(preset.Id)).Text;
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}
}
