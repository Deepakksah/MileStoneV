using System;
using System.Globalization;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.HardwareSetup;

public static class NamingScheme
{
	public static string FindHardwareName(HardwareNameTemplate hardwareNameTemplate, HardwareSetupSettings hardwareSetupSettings)
	{
		if (hardwareSetupSettings == null)
		{
			throw new ArgumentNullException("hardwareSetupSettings");
		}
		string result = string.Empty;
		if (hardwareNameTemplate == HardwareNameTemplate.Default)
		{
			hardwareNameTemplate = ((!hardwareSetupSettings.IsInterconnectedProduct || hardwareSetupSettings.IsArculesMipDriver) ? HardwareNameTemplate.ModelAddress : HardwareNameTemplate.InterconnectedName);
		}
		switch (hardwareNameTemplate)
		{
		case HardwareNameTemplate.InterconnectedName:
			result = hardwareSetupSettings.InterconnectedHardwareName;
			break;
		case HardwareNameTemplate.ModelAddress:
			result = string.Format(CultureInfo.CurrentCulture, HardwareSetupResources.NamingHardwareTemplateModelAddress, hardwareSetupSettings.IsMipDriver ? hardwareSetupSettings.InterconnectedHardwareName : hardwareSetupSettings.Model, hardwareSetupSettings.Address.Host);
			break;
		case HardwareNameTemplate.Model:
			result = hardwareSetupSettings.Model;
			break;
		case HardwareNameTemplate.HostedHardwareName:
			if (hardwareSetupSettings.HostedHardware == null)
			{
				ThrowInvalidOperationExceptionHostedHardwareWasNull();
			}
			result = hardwareSetupSettings.HostedHardware.Name;
			break;
		}
		return result;
	}

	private static void ThrowInvalidOperationExceptionHostedHardwareWasNull()
	{
		throw new InvalidOperationException("The template 'HardwareTemplate.HostedHardwareName' was selected for item where HostedHardware property was null.");
	}

	public static string FindDeviceName(DeviceNameTemplate deviceNameTemplate, HardwareSetupSettings hardwareSetupSettings, DeviceSetupSettings deviceSetupSettings)
	{
		if (hardwareSetupSettings == null)
		{
			throw new ArgumentNullException("hardwareSetupSettings");
		}
		if (deviceSetupSettings == null)
		{
			throw new ArgumentNullException("deviceSetupSettings");
		}
		return FindDeviceName(deviceNameTemplate, deviceSetupSettings.DeviceHwid, hardwareSetupSettings.Name, hardwareSetupSettings.Address, hardwareSetupSettings.IsIcpOrMipDriver, deviceSetupSettings.InterconnectedProductName);
	}

	public static string FindDeviceName(DeviceNameTemplate deviceNameTemplate, DeviceHwid deviceHwid, string hardwareName, Uri hardwareAddress, bool isInterconnected, string interconnectedProductName)
	{
		if (deviceHwid == null)
		{
			throw new ArgumentNullException("deviceHwid");
		}
		if (hardwareName == null)
		{
			throw new ArgumentNullException("hardwareName");
		}
		if (hardwareAddress == null)
		{
			throw new ArgumentNullException("hardwareAddress");
		}
		if (interconnectedProductName == null)
		{
			throw new ArgumentNullException("interconnectedProductName");
		}
		int num = deviceHwid.Index + 1;
		string text = FindDeviceTypeString(deviceHwid.Type);
		string result = string.Empty;
		if (deviceNameTemplate == DeviceNameTemplate.Default)
		{
			deviceNameTemplate = ((!isInterconnected) ? DeviceNameTemplate.HardwareNameDeviceTypeIndex : DeviceNameTemplate.InterconnectedName);
		}
		switch (deviceNameTemplate)
		{
		case DeviceNameTemplate.InterconnectedName:
			result = string.Format(CultureInfo.CurrentCulture, HardwareSetupResources.NamingDeviceTemplateInterconnectedName, hardwareName, interconnectedProductName);
			break;
		case DeviceNameTemplate.HardwareNameDeviceTypeIndex:
			result = string.Format(CultureInfo.CurrentCulture, HardwareSetupResources.NamingDeviceTemplateHwNameDeviceTypeIdx, hardwareName, text, num);
			break;
		case DeviceNameTemplate.DeviceTypeIndexHardwareName:
			result = string.Format(CultureInfo.CurrentCulture, HardwareSetupResources.NamingDeviceTemplateDeviceTypeIdxHwName, text, num, hardwareName);
			break;
		case DeviceNameTemplate.HardwareAddressDeviceTypeIndex:
			result = string.Format(CultureInfo.CurrentCulture, HardwareSetupResources.NamingDeviceTemplateHwAddressDeviceTypeIdx, hardwareAddress.Host, text, num);
			break;
		}
		return result;
	}

	public static string FindDeviceTypeString(DeviceTypeEnum deviceType)
	{
		string result = string.Empty;
		switch (deviceType)
		{
		case DeviceTypeEnum.Camera:
			result = HardwareSetupResources.DeviceTypeCamera;
			break;
		case DeviceTypeEnum.AudioInput:
			result = HardwareSetupResources.DeviceTypeMicrophone;
			break;
		case DeviceTypeEnum.AudioOutput:
			result = HardwareSetupResources.DeviceTypeSpeaker;
			break;
		case DeviceTypeEnum.Metadata:
			result = HardwareSetupResources.DeviceTypeMetadata;
			break;
		case DeviceTypeEnum.Input:
			result = HardwareSetupResources.DeviceTypeInput;
			break;
		case DeviceTypeEnum.Output:
			result = HardwareSetupResources.DeviceTypeOutput;
			break;
		}
		return result;
	}
}
