using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Drivers;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIFisheyeLens")]
[EntityName("ConfigAPIFisheyeLensFolder")]
public class FisheyeConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
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
		IReadOnlyCollection<LensProfile> lensProfiles = QueryLensProfiles(requestContext);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, lensProfiles);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		bool enabled = item.EnableProperty.Enabled;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegisteredPanomorphLens", out string _);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Orientation", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegisteredPanomorphLensNumber", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FisheyeFieldOfView", out string value4);
		try
		{
			ImmerVisionSettings immerVisionSettings = QueryFisheyeSettings(requestContext, path.Id);
			immerVisionSettings.Enabled = enabled;
			immerVisionSettings.FieldOfView = double.Parse(value4, CultureInfo.InvariantCulture);
			immerVisionSettings.RegisteredPanomorphLensNumber = value3;
			immerVisionSettings.Position = (DevicePosition)Enum.Parse(typeof(DevicePosition), value2, ignoreCase: false);
			StoreFisheyeSettings(requestContext, path.Id, immerVisionSettings, enabled);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = ConvertToFisheyeConfigurationItem(parentPath, immerVisionSettings, path.Id, lensProfiles);
			return validateResult;
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(parentPath.Text);
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
		IReadOnlyCollection<LensProfile> lensProfiles = QueryLensProfiles(requestContext);
		return ValidateItemInternal(requestContext, item, path, parentPath, lensProfiles);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, IEnumerable<LensProfile> lensProfiles)
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
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegisteredPanomorphLens", out string _);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Orientation", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegisteredPanomorphLensNumber", out string number);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FisheyeFieldOfView", out string value3);
		if (value2 != "Ceiling" && value2 != "Wall" && value2 != "Ground")
		{
			throw new PropertyInvalidException("Invalid value", "Orientation");
		}
		if (lensProfiles != null && lensProfiles.Any() && lensProfiles.FirstOrDefault((LensProfile p) => p.RegisteredPanomorphLensNumber == number) == null && number != "TESTVALUE")
		{
			throw new PropertyInvalidException("Invalid lens number", "RegisteredPanomorphLensNumber");
		}
		if (!int.TryParse(value3, out var result) || result < 80 || result > 360)
		{
			throw new PropertyInvalidException("Invalid value", "FisheyeFieldOfView");
		}
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
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
			Device device = Device.QueryDeviceById(requestContext, path.Id);
			if (device == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (device.DeviceType != DeviceType.Camera)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (path.ParentItemType != "Camera" || !HasFisheyeSupport(device))
			{
				throw new NotSupportedException();
			}
			return new List<ConfigurationItem> { GetItem(requestContext, ConfigurationItemPath.CreateItemPath("FisheyeLens", path.Id)) }.ToArray();
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
		Device obj = Device.QueryDeviceById(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text);
		if (obj.DeviceType != DeviceType.Camera)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (!HasFisheyeSupport(obj))
		{
			throw new NotSupportedException("Camera does not support fisheye");
		}
		ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("Camera", path.Id);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("FisheyeLensFolder", parentPath);
		}
		return ConvertToFisheyeConfigurationItem(ConfigurationItemPath.CreateFolderPath(parentPath, "FisheyeLensFolder"), QueryFisheyeSettings(requestContext, path.Id), lensProfiles: QueryLensProfiles(requestContext), cameraId: path.Id);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new ArgumentException("Unknown methodId:" + method);
	}

	private static ImmerVisionSettings QueryFisheyeSettings(IGeminiRequestContext requestContext, string cameraId)
	{
		DeviceRecorderPlugin deviceRecorderPlugin = DeviceRecorderPlugin.QueryEnabledPluginsByTypeAndId(requestContext, GlobalIDs.ImmerVisionPlugInGuid, new Guid(cameraId));
		if (deviceRecorderPlugin != null)
		{
			ImmerVisionSettings immerVisionSettings = ImmerVisionSettings.FromXml(deviceRecorderPlugin.Settings);
			if (immerVisionSettings.FieldOfView < 0.0)
			{
				immerVisionSettings.FieldOfView = 80.0;
			}
			if (immerVisionSettings.FieldOfView > 360.0)
			{
				immerVisionSettings.FieldOfView = 360.0;
			}
			return immerVisionSettings;
		}
		return new ImmerVisionSettings();
	}

	private static void StoreFisheyeSettings(IGeminiRequestContext requestContext, string cameraId, ImmerVisionSettings settings, bool enabled)
	{
		DeviceRecorderPlugin.UpdateDeviceFisheyeSettings(requestContext, new Guid(cameraId), settings.ToXml(), enabled);
	}

	private static IReadOnlyCollection<LensProfile> QueryLensProfiles(IGeminiRequestContext requestContext)
	{
		IReadOnlyList<ImmerVisionLensDescription> lenses = ImmerVisionLenses.Lenses;
		ExternalPluginsData externalPluginsData = ExternalPluginsData.QueryExternalSmartClientPlugin(requestContext, GlobalIDs.ImmerVisionPlugInGuid);
		if (externalPluginsData != null)
		{
			ImmerVisionWrapper immerVisionWrapper = new ImmerVisionWrapper(externalPluginsData.Data);
			return ImmerVisionLenses.MergeProfiles(lenses, immerVisionWrapper.LoadLensProfiles());
		}
		return ImmerVisionLenses.MergeProfiles(lenses, new ReadOnlyCollection<LensProfile>(new List<LensProfile>()));
	}

	public static bool HasFisheyeSupport(Device cameraDevice)
	{
		return !cameraDevice.SettingUtilities.IsPtzEnabled;
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

	private static ConfigurationItem ConvertToFisheyeConfigurationItem(ConfigurationItemPath parentPath, ImmerVisionSettings settings, string cameraId, IReadOnlyCollection<LensProfile> lensProfiles)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "FisheyeLens";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.FisheyeName;
		configurationItem.EnableProperty = new EnablePropertyInfo
		{
			DisplayName = ConfigurationApiResources.PropertyGeneralEnabled,
			Enabled = settings.Enabled,
			TranslationId = "PropertyGeneralEnabled",
			UIToFollowEnabled = true
		};
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath("FisheyeLens", cameraId).Text;
		configurationItem.Properties = new Property[4]
		{
			FisheyePropertyDefinition.LensTypeProperty(),
			FisheyePropertyDefinition.CameraOrientationProperty(settings.Position),
			FisheyePropertyDefinition.RegisteredPanomorphLensNumberProperty(settings.RegisteredPanomorphLensNumber ?? "Generic", lensProfiles),
			FisheyePropertyDefinition.FieldOfViewProperty(settings.FieldOfView)
		};
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}
}
