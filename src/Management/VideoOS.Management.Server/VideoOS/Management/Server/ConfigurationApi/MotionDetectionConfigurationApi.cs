using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMotionDetection")]
[EntityName("ConfigAPIMotionDetectionFolder")]
public class MotionDetectionConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string DetectionEnabled = "DetectionEnabled";

	public const string ManualSensitivityEnabled = "ManualSensitivityEnabled";

	public const string KeyframesOnly = "KeyframesOnly";

	public const string GenerateMotionMetadata = "GenerateMotionMetadata";

	public const string ManualSensitivity = "ManualSensitivity";

	public const string Threshold = "Threshold";

	public const string ProcessTime = "ProcessTime";

	public const string DetectionMethod = "DetectionMethod";

	public const string UseExcludeRegions = "UseExcludeRegions";

	public const string GridSize = "GridSize";

	public const string ExcludeRegions = "ExcludeRegions";

	public const string HardwareAccelerationMode = "HardwareAccelerationMode";

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
		DeviceMotionDetection deviceMotionDetection = DeviceMotionDetection.QueryMotionDetectionById(requestContext, id);
		if (deviceMotionDetection == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (string.Equals(itemType, "MotionDetectionFolder", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[1] { ConvertToMotionDetectionConfigurationItem(path, deviceMotionDetection) };
		}
		if (string.Equals(itemType, "MotionDetection", StringComparison.OrdinalIgnoreCase))
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
		if (string.Equals(path.ItemType, "MotionDetectionFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("MotionDetectionFolder", parentPath);
		}
		string id = path.Id;
		DeviceMotionDetection deviceMotionDetection = DeviceMotionDetection.QueryMotionDetectionById(requestContext, id);
		if (deviceMotionDetection == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToMotionDetectionConfigurationItem(ConfigurationItemPath.CreateFolderPath("Camera", deviceMotionDetection.Id, "MotionDetectionFolder"), deviceMotionDetection);
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
		if (!string.Equals(path.ItemType, "MotionDetection", StringComparison.Ordinal))
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var detectionEnabled, out var manualSensitivityEnabled, out var keyFramesOnly, out var generateMotionMetadata, out var manualSensitivity, out var threshold, out var processTime, out var detectionMethod, out var hardwareAccelerationMode, out var useExcludeRegions, out var gridSize, out var excludeRegions);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			DeviceMotionDetection motionDetection = DeviceMotionDetection.UpdateMotionDetection(requestContext, path.Id, detectionEnabled, manualSensitivityEnabled, manualSensitivity, threshold, keyFramesOnly, processTime, detectionMethod, hardwareAccelerationMode, generateMotionMetadata, useExcludeRegions, gridSize, excludeRegions);
			ConfigurationItem resultItem = ConvertToMotionDetectionConfigurationItem(parentPath, motionDetection);
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
		if (string.Equals(path.ItemType, "MotionDetection", StringComparison.Ordinal))
		{
			if (DeviceMotionDetection.QueryMotionDetectionById(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			bool? detectionEnabled;
			bool? manualSensitivityEnabled;
			bool? keyFramesOnly;
			bool? generateMotionMetadata;
			int? manualSensitivity;
			int? threshold;
			ProcessTime.ProcessTimeType? processTime;
			DetectionMethod.DetectionMethodType? detectionMethod;
			HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode;
			bool? useExcludeRegions;
			GridSize.GridSizeType? gridSize;
			string excludeRegions;
			return ValidateItemInternal(requestContext, item, path, out detectionEnabled, out manualSensitivityEnabled, out keyFramesOnly, out generateMotionMetadata, out manualSensitivity, out threshold, out processTime, out detectionMethod, out hardwareAccelerationMode, out useExcludeRegions, out gridSize, out excludeRegions);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out bool? detectionEnabled, out bool? manualSensitivityEnabled, out bool? keyFramesOnly, out bool? generateMotionMetadata, out int? manualSensitivity, out int? threshold, out ProcessTime.ProcessTimeType? processTime, out DetectionMethod.DetectionMethodType? detectionMethod, out HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode, out bool? useExcludeRegions, out GridSize.GridSizeType? gridSize, out string excludeRegions)
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
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "MotionDetection", StringComparison.Ordinal))
		{
			if (item.EnableProperty == null)
			{
				detectionEnabled = null;
			}
			else
			{
				detectionEnabled = item.EnableProperty.Enabled;
			}
			manualSensitivityEnabled = null;
			keyFramesOnly = null;
			generateMotionMetadata = null;
			manualSensitivity = null;
			threshold = null;
			processTime = null;
			detectionMethod = null;
			useExcludeRegions = null;
			gridSize = null;
			excludeRegions = null;
			hardwareAccelerationMode = null;
			try
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManualSensitivityEnabled", out manualSensitivityEnabled);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("KeyframesOnly", out keyFramesOnly);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("GenerateMotionMetadata", out generateMotionMetadata);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ManualSensitivity", out manualSensitivity);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Threshold", out threshold);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ProcessTime", out processTime);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DetectionMethod", out detectionMethod);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UseExcludeRegions", out useExcludeRegions);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("GridSize", out gridSize);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ExcludeRegions", out excludeRegions);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("HardwareAccelerationMode", out hardwareAccelerationMode);
				}
				if (string.IsNullOrEmpty(excludeRegions))
				{
					excludeRegions = null;
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
				DeviceMotionDetection.UpdateMotionDetectionValidation(requestContext, path.Id, manualSensitivity, threshold, processTime, detectionMethod, hardwareAccelerationMode, gridSize, excludeRegions);
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

	private static ConfigurationItem ConvertToMotionDetectionConfigurationItem(ConfigurationItemPath parentPath, DeviceMotionDetection motionDetection)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "MotionDetection";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeMotionDetectionFolder;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, motionDetection.Id).Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(motionDetection.MotionDetectionEnabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(motionDetection.Id));
		list.Add(MotionDetectionPropertyDefinition.ManualSensitivityEnabled(motionDetection.ManualSensitivityEnabled));
		list.Add(MotionDetectionPropertyDefinition.ManualSensitivity(motionDetection.MotionPixThreshold));
		list.Add(MotionDetectionPropertyDefinition.Threshold(motionDetection.MotionPixCountThreshold));
		list.Add(MotionDetectionPropertyDefinition.KeyframesOnly(motionDetection.KeyFramesOnly));
		list.Add(MotionDetectionPropertyDefinition.ProcessTime(motionDetection.ProcessTime));
		list.Add(MotionDetectionPropertyDefinition.DetectionMethod(motionDetection.DetectionMethod));
		list.Add(MotionDetectionPropertyDefinition.GenerateMotionMetadata(motionDetection.GenerateMotionMetadata));
		list.Add(MotionDetectionPropertyDefinition.UseExcludeRegions(motionDetection.UseExcludeRegions));
		list.Add(MotionDetectionPropertyDefinition.GridSize(motionDetection.GridSize));
		list.Add(MotionDetectionPropertyDefinition.ExcludeRegions(motionDetection.MotionDetectionExcludeRegions));
		list.Add(MotionDetectionPropertyDefinition.HardwareAccelerationMode(motionDetection.HardwareAccelerationMode));
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
