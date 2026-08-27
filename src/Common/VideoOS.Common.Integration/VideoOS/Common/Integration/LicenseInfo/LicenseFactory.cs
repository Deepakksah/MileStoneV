using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.LicenseInfo.Internal;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public static class LicenseFactory
{
	public static bool IsChannelBasedLicensing { get; set; }

	public static ILicenseActivationService RetrieveLicenseActivationService(ILogger logger, Uri activationUrl, int timeout)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (activationUrl == null)
		{
			throw new ArgumentNullException("activationUrl");
		}
		if (timeout < 1)
		{
			throw new ArgumentOutOfRangeException("timeout");
		}
		return new LicenseActivationService(new HttpPost(), logger, activationUrl, timeout);
	}

	public static ILicenseLoader CreateLicenseLoader()
	{
		return new LicenseLoader();
	}

	public static ILicenseImporter CreateLicenseImporter()
	{
		return new LicenseImporter();
	}

	public static ILicenseReader CreateLicenseReader()
	{
		return new LicenseReader();
	}

	public static IDeviceInfo CreateNewDeviceInfo(Guid id, Guid originalDeviceId, string deviceType, int deviceIndex, bool enabled, IHardwareInfo hardwareInfo)
	{
		return new DeviceInfo(id, originalDeviceId, deviceType, deviceIndex, enabled, hardwareInfo);
	}

	public static IDeviceInfo CreateNewDeviceInfo(string id, string originalDeviceId, string deviceType, int deviceIndex, bool enabled, IHardwareInfo hardwareInfo)
	{
		return CreateNewDeviceInfo(new Guid(id), new Guid(originalDeviceId), deviceType, deviceIndex, enabled, hardwareInfo);
	}

	public static ILicenseReportGenerator CreateLicenseReportGenerator(LicenseReportType licenseReportType)
	{
		return licenseReportType switch
		{
			LicenseReportType.Warning => new LicenseWarningReportGenerator(), 
			LicenseReportType.Information => new LicenseInformationReportGenerator(), 
			_ => throw new InvalidOperationException("Invalid report type"), 
		};
	}

	public static IFeature CreateNewFeature(string featureName)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentNullException("featureName");
		}
		return new Feature(featureName);
	}

	public static IFeedDictionary CreateNewFeedDictionary()
	{
		return new FeedDictionary();
	}

	public static IRecorderInfo CreateRecorder(Guid id, bool enabled, string devicePack)
	{
		return new RecorderInfo(id, enabled, devicePack);
	}

	public static IHardwareDictionary CreateNewHardwareDictionary()
	{
		return new HardwareDictionary();
	}

	public static IHardwareInfo CreateNewHardwareInfo(Guid id, Guid recorderId, string name, bool enabled, string macAddress, string serialNumber, string driverName, string driverVersion, int driverNumber, string detectedModel, string vendorProductId, bool isInterconnected, List<IDeviceInfo> devices)
	{
		if (IsChannelBasedLicensing)
		{
			return new HardwareInfoChannel(id, recorderId, name, enabled, macAddress, serialNumber, driverName, driverVersion, driverNumber, detectedModel, vendorProductId, isInterconnected, devices);
		}
		return new HardwareInfo(id, recorderId, name, enabled, macAddress, serialNumber, driverName, driverVersion, driverNumber, detectedModel, vendorProductId, isInterconnected, devices);
	}

	public static ILicenseProperty CreateNewLicenseProperty(string propertyType, string name, string value)
	{
		return new LicenseProperty(propertyType, name, value);
	}

	public static IPort CreateNewPort(string name, int count)
	{
		return new Port(name, count);
	}

	public static IPortDictionary CreateNewPortDictionary()
	{
		return new PortDictionary();
	}

	public static IRecordersRequiringLicenseUpdateDictionary CreateRecordersRequiringLicensingDictionary()
	{
		return new RecordersRequiringLicenseUpdateDictionary();
	}

	public static IReplacementLimit CreateNewReplacementLimit()
	{
		return new ReplacementLimit();
	}

	public static IReplacementLimit CreateNewReplacementLimit(int minimum, int maximum, int percentage)
	{
		return new ReplacementLimit(minimum, maximum, percentage);
	}

	public static ITotalLicense CreateNewTotalLicense(int hardwareCount, int cameraCount, int interconnectedCameraCount, int arculesCameraCount)
	{
		return new TotalLicense(hardwareCount, cameraCount, interconnectedCameraCount, arculesCameraCount);
	}

	public static ILicenseCalculationResult CreateLicenseCalculationResult(string macAddress, string modelName, string driverName, string hardwareName, int channelLicenses)
	{
		return new LicenseCalculationResult(macAddress, modelName, driverName, hardwareName, channelLicenses);
	}

	public static ILicenseCalculator CreateLicenseCalculator()
	{
		return new LicenseCalculator();
	}

	public static IRecorderLicenseInfo CreateRecorderLicenseInfo(Guid id, DateTime lastModified)
	{
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			lastModified = lastModified.ToUniversalTime();
		}
		return new RecorderLicenseInfo(id, lastModified);
	}

	public static IRecorderLicenseInfo CreateRecorderLicenseInfo(Guid id)
	{
		return new RecorderLicenseInfo(id, DateTime.UtcNow);
	}

	public static IHardwareLicenseInfo CreateHardwareLicenseInfo(IHardwareInfo hardwareInfo, DateTime lastModified, long license)
	{
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			lastModified = lastModified.ToUniversalTime();
		}
		return new HardwareLicenseInfo(hardwareInfo, lastModified, license);
	}

	public static IDeviceLicenseInfo CreateDeviceLicenseInfo(IDeviceInfo deviceInfo, DateTime lastModified, long license)
	{
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			lastModified = lastModified.ToUniversalTime();
		}
		return new DeviceLicenseInfo(deviceInfo, lastModified, license);
	}

	public static IHardwareDeviceItem CreateHardwareDeviceItem(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		return new HardwareDeviceItem(hardwareInfo.MacAddress, deviceInfo.UniqueIdentifier);
	}

	public static IHardwareDeviceItem CreateHardwareDeviceItem(string macAddress, string deviceUniqueIdentifier)
	{
		return new HardwareDeviceItem(macAddress, deviceUniqueIdentifier);
	}
}
