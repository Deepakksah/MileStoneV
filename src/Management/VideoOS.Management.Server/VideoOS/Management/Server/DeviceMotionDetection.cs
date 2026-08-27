using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public class DeviceMotionDetection
{
	public string Id { get; set; }

	public bool MotionDetectionEnabled { get; set; }

	public bool ManualSensitivityEnabled { get; set; }

	public int MotionPixThreshold { get; set; }

	public int MotionPixCountThreshold { get; set; }

	public bool KeyFramesOnly { get; set; }

	public ProcessTime.ProcessTimeType ProcessTime { get; set; }

	public DetectionMethod.DetectionMethodType DetectionMethod { get; set; }

	public bool GenerateMotionMetadata { get; set; }

	public bool UseExcludeRegions { get; set; }

	public GridSize.GridSizeType GridSize { get; set; }

	public string MotionDetectionExcludeRegions { get; set; }

	public DateTime LastModified { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public HardwareAccelerationMode.HardwareAccelerationModeType HardwareAccelerationMode { get; set; }

	public static DeviceMotionDetection QueryMotionDetectionById(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId))
		{
			return null;
		}
		DeviceMotionDetection result = null;
		DeviceMotionDetectionDataContract deviceMotionDetectionDataContract = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			deviceMotionDetectionDataContract = deviceComponent.QueryDeviceMotionDetection(deviceId);
		}
		if (deviceMotionDetectionDataContract != null)
		{
			result = PopulateMotionDetectionSettings(deviceMotionDetectionDataContract);
		}
		return result;
	}

	public static DeviceMotionDetection[] QueryMotionDetectionsByIds(IGeminiRequestContext requestContext, string[] deviceIds)
	{
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		DeviceMotionDetectionDataContract[] array = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			array = deviceComponent.QueryDeviceMotionDetections(deviceIds);
		}
		List<DeviceMotionDetection> list = new List<DeviceMotionDetection>();
		DeviceMotionDetectionDataContract[] array2 = array;
		foreach (DeviceMotionDetectionDataContract deviceMotionDetectionDataContract in array2)
		{
			if (deviceMotionDetectionDataContract != null)
			{
				DeviceMotionDetection item = PopulateMotionDetectionSettings(deviceMotionDetectionDataContract);
				list.Add(item);
			}
		}
		return FilterMotionDetectionsBySecurity(requestContext, list.ToArray());
	}

	private static DeviceMotionDetection[] FilterMotionDetectionsBySecurity(IGeminiRequestContext requestContext, DeviceMotionDetection[] motionDetections)
	{
		if (motionDetections == null)
		{
			return new DeviceMotionDetection[0];
		}
		List<DeviceMotionDetection> list = new List<DeviceMotionDetection>();
		foreach (DeviceMotionDetection deviceMotionDetection in motionDetections)
		{
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceMotionDetection.Id))
			{
				list.Add(deviceMotionDetection);
			}
		}
		return list.ToArray();
	}

	public static DeviceMotionDetection UpdateMotionDetection(IGeminiRequestContext requestContext, string deviceId, bool? motionDetectionEnabled, bool? manualSensitivityEnabled, int? motionPixTreeshold, int? motionPixCountTreeshold, bool? keyFramesOnly, ProcessTime.ProcessTimeType? processTime, DetectionMethod.DetectionMethodType? detectionMethod, HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode, bool? generateMotionMetadata, bool? useExcludeRegions, GridSize.GridSizeType? gridSize, string motionDetectionExcludeRegions)
	{
		Guid sessionId = UpdateMotionDetectionInternal(requestContext, deviceId, motionDetectionEnabled, manualSensitivityEnabled, motionPixTreeshold, motionPixCountTreeshold, keyFramesOnly, processTime, detectionMethod, hardwareAccelerationMode, generateMotionMetadata, useExcludeRegions, gridSize, motionDetectionExcludeRegions, 5);
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return QueryMotionDetectionById(requestContext, deviceId);
	}

	public static Guid UpdateMotionDetectionInternal(IGeminiRequestContext requestContext, string deviceId, bool? motionDetectionEnabled, bool? manualSensitivityEnabled, int? motionPixTreeshold, int? motionPixCountTreeshold, bool? keyFramesOnly, ProcessTime.ProcessTimeType? processTime, DetectionMethod.DetectionMethodType? detectionMethod, HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode, bool? generateMotionMetadata, bool? useExcludeRegions, GridSize.GridSizeType? gridSize, string motionDetectionExcludeRegions, int numberOfRetries)
	{
		UpdateMotionDetectionValidation(requestContext, deviceId, motionPixTreeshold, motionPixCountTreeshold, processTime, detectionMethod, hardwareAccelerationMode, gridSize, motionDetectionExcludeRegions);
		DeviceMotionDetectionDataContract deviceMotionDetectionDataContract = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			deviceMotionDetectionDataContract = deviceComponent.QueryDeviceMotionDetection(deviceId);
		}
		if (deviceMotionDetectionDataContract == null)
		{
			throw new DeviceNotFoundException();
		}
		MergeMotionDetectionSettings(deviceMotionDetectionDataContract, motionDetectionEnabled, manualSensitivityEnabled, motionPixTreeshold, motionPixCountTreeshold, keyFramesOnly, processTime, detectionMethod, hardwareAccelerationMode, generateMotionMetadata, useExcludeRegions, gridSize, motionDetectionExcludeRegions);
		Guid guid = Guid.NewGuid();
		try
		{
			using IDeviceComponent deviceComponent2 = requestContext.CreateDeviceComponent();
			deviceComponent2.UpdateDeviceMotionDetection(deviceId, deviceMotionDetectionDataContract.LastModified, deviceMotionDetectionDataContract.MotionDetectionXml, deviceMotionDetectionDataContract.MotionDetectionEnabled, guid);
			return guid;
		}
		catch (DeviceNotRecentVersionException)
		{
			if (numberOfRetries <= 0)
			{
				throw;
			}
			return UpdateMotionDetectionInternal(requestContext, deviceId, motionDetectionEnabled, manualSensitivityEnabled, motionPixTreeshold, motionPixCountTreeshold, keyFramesOnly, processTime, detectionMethod, hardwareAccelerationMode, generateMotionMetadata, useExcludeRegions, gridSize, motionDetectionExcludeRegions, numberOfRetries - 1);
		}
	}

	public static DeviceMotionDetection PopulateMotionDetectionSettings(DeviceMotionDetectionDataContract deviceMotionDetectionDataContract)
	{
		DeviceMotionDetection deviceMotionDetection = new DeviceMotionDetection
		{
			Id = deviceMotionDetectionDataContract.Id,
			MotionDetectionEnabled = deviceMotionDetectionDataContract.MotionDetectionEnabled,
			LastModified = deviceMotionDetectionDataContract.LastModified,
			HardwareId = deviceMotionDetectionDataContract.HardwareId,
			RecordingServerId = deviceMotionDetectionDataContract.RecordingServerId
		};
		MotionDetectionSettings motionDetectionSettings = new MotionDetectionSettings();
		motionDetectionSettings.Xml2Config(deviceMotionDetectionDataContract.MotionDetectionXml);
		deviceMotionDetection.ManualSensitivityEnabled = motionDetectionSettings.ManualSensitivity;
		deviceMotionDetection.MotionPixThreshold = motionDetectionSettings.MotionPixThreshold;
		deviceMotionDetection.MotionPixCountThreshold = motionDetectionSettings.MotionPixCountThreshold;
		deviceMotionDetection.KeyFramesOnly = motionDetectionSettings.KeyframesOnly;
		deviceMotionDetection.ProcessTime = VideoOS.Management.Common.ProcessTime.ConvertInt32ToProcessTime(motionDetectionSettings.TimeThresholdMs);
		deviceMotionDetection.DetectionMethod = VideoOS.Management.Common.DetectionMethod.ConvertInt32ToDetectionMethod(motionDetectionSettings.PixelStep);
		deviceMotionDetection.GenerateMotionMetadata = motionDetectionSettings.GenerateSearchData;
		deviceMotionDetection.UseExcludeRegions = motionDetectionSettings.UseRegions;
		deviceMotionDetection.HardwareAccelerationMode = VideoOS.Management.Common.HardwareAccelerationMode.ConvertFromInt32((int)motionDetectionSettings.HardwareAcceleration);
		deviceMotionDetection.GridSize = VideoOS.Management.Common.GridSize.ConvertInt32ToGridSize(motionDetectionSettings.MotionExcludedRegions.RegionsXAxis);
		int num = VideoOS.Management.Common.GridSize.ConvertGridSizeToInt32(deviceMotionDetection.GridSize);
		deviceMotionDetection.MotionDetectionExcludeRegions = new string('0', num * num);
		try
		{
			if (motionDetectionSettings.MotionExcludedRegions.HasFilledCell)
			{
				byte[] regionsAsByteArray = motionDetectionSettings.MotionExcludedRegions.GetRegionsAsByteArray();
				StringBuilder stringBuilder = new StringBuilder(deviceMotionDetection.MotionDetectionExcludeRegions);
				for (int i = 0; i < regionsAsByteArray.Length; i++)
				{
					if (regionsAsByteArray[i] == 1)
					{
						stringBuilder[i] = '1';
					}
				}
				deviceMotionDetection.MotionDetectionExcludeRegions = stringBuilder.ToString();
			}
		}
		catch (InvalidOperationException)
		{
			deviceMotionDetection.MotionDetectionExcludeRegions = new string('0', motionDetectionSettings.MotionExcludedRegions.RegionsXAxis * motionDetectionSettings.MotionExcludedRegions.RegionsXAxis);
		}
		return deviceMotionDetection;
	}

	public static void MergeMotionDetectionSettings(DeviceMotionDetectionDataContract original, bool? motionDetectionEnabled, bool? manualSensitivityEnabled, int? motionPixTreeshold, int? motionPixCountTreeshold, bool? keyFramesOnly, ProcessTime.ProcessTimeType? processTime, DetectionMethod.DetectionMethodType? detectionMethod, HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode, bool? generateMotionMetadata, bool? useExcludeRegions, GridSize.GridSizeType? gridSize, string motionDetectionExcludeRegions)
	{
		MotionDetectionSettings motionDetectionSettings = new MotionDetectionSettings();
		motionDetectionSettings.Xml2Config(original.MotionDetectionXml);
		if (motionDetectionEnabled.HasValue)
		{
			motionDetectionSettings.Enabled = motionDetectionEnabled.Value;
			original.MotionDetectionEnabled = motionDetectionEnabled.Value;
		}
		if (manualSensitivityEnabled.HasValue)
		{
			motionDetectionSettings.ManualSensitivity = manualSensitivityEnabled.Value;
		}
		if (motionPixTreeshold.HasValue)
		{
			motionDetectionSettings.MotionPixThreshold = motionPixTreeshold.Value;
		}
		if (motionPixCountTreeshold.HasValue)
		{
			motionDetectionSettings.MotionPixCountThreshold = motionPixCountTreeshold.Value;
		}
		if (keyFramesOnly.HasValue)
		{
			motionDetectionSettings.KeyframesOnly = keyFramesOnly.Value;
		}
		if (processTime.HasValue)
		{
			motionDetectionSettings.TimeThresholdMs = VideoOS.Management.Common.ProcessTime.ConvertProcessTimeToInt32(processTime.Value);
		}
		if (detectionMethod.HasValue)
		{
			motionDetectionSettings.PixelStep = VideoOS.Management.Common.DetectionMethod.ConvertDetectionMethodToIn32(detectionMethod.Value);
		}
		if (hardwareAccelerationMode.HasValue)
		{
			motionDetectionSettings.HardwareAcceleration = hardwareAccelerationMode.Value;
		}
		if (generateMotionMetadata.HasValue)
		{
			motionDetectionSettings.GenerateSearchData = generateMotionMetadata.Value;
		}
		if (gridSize.HasValue)
		{
			VideoOS.Management.Common.GridSize.SetGridSize(gridSize.Value, motionDetectionSettings.MotionExcludedRegions.RegionsXAxis, motionDetectionSettings.MotionExcludedRegions.IncreaseNumberOfRegions, motionDetectionSettings.MotionExcludedRegions.InitializeGridArea);
		}
		if (useExcludeRegions.HasValue)
		{
			motionDetectionSettings.UseRegions = useExcludeRegions.Value;
		}
		if (motionDetectionExcludeRegions != null)
		{
			byte[] regionsAsByteArray = motionDetectionSettings.MotionExcludedRegions.GetRegionsAsByteArray();
			if (regionsAsByteArray.Length != 0 && regionsAsByteArray.Length != motionDetectionExcludeRegions.Length)
			{
				throw new ArgumentException("Exclude regions doesn't match grid size", "motionDetectionExcludeRegions");
			}
			for (int i = 0; i < motionDetectionExcludeRegions.Length; i++)
			{
				char num = motionDetectionExcludeRegions[i];
				int x = i % motionDetectionSettings.MotionExcludedRegions.RegionsXAxis;
				int y = i / motionDetectionSettings.MotionExcludedRegions.RegionsXAxis;
				bool filled = num == '1';
				motionDetectionSettings.MotionExcludedRegions.SetRegion(x, y, filled);
			}
		}
		original.MotionDetectionXml = motionDetectionSettings.Config2Xml();
	}

	public static void UpdateMotionDetectionValidation(IGeminiRequestContext requestContext, string motionDetectionId, int? manualSensitivity, int? threshold, ProcessTime.ProcessTimeType? processTime, DetectionMethod.DetectionMethodType? detectionMethod, HardwareAccelerationMode.HardwareAccelerationModeType? hardwareAccelerationMode, GridSize.GridSizeType? gridSize, string excludeRegions)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", motionDetectionId);
		Validation.GridSize(gridSize, "gridSize", allowNull: true);
		Validation.MotionDetectionThresholdMilliseconds(processTime, "processTime", allowNull: true);
		Validation.MotionDetectionMethod(detectionMethod, "detectionMethod", allowNull: true);
		if (!string.IsNullOrEmpty(excludeRegions))
		{
			int? gridLength = null;
			if (gridSize.HasValue)
			{
				gridLength = VideoOS.Management.Common.GridSize.CalculateGridLength(gridSize.Value);
			}
			Validation.GridRegions(excludeRegions, "excludeRegions", gridLength);
		}
		if (manualSensitivity.HasValue)
		{
			Validation.ValidateInt32(manualSensitivity, "manualSensitivity", 0, 300, allowNull: true);
		}
		if (threshold.HasValue)
		{
			Validation.ValidateInt32(threshold, "threshold", 0, 10000, allowNull: true);
		}
		Validation.CheckObjectId(ref motionDetectionId, "motionDetectionId", allowNullOrEmpty: false);
		Validation.MotionDetectionHardwareAcceleration(hardwareAccelerationMode, "hardwareAccelerationMode", allowNull: true);
	}
}
