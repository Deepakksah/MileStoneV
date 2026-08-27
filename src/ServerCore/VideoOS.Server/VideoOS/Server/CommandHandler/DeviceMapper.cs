using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.HardwareSetup;

namespace VideoOS.Server.CommandHandler;

public class DeviceMapper : DeviceMapperBase
{
	private readonly ReplaceHardwareLocals locals;

	public DeviceMapper(ReplaceHardwareLocals locals, List<ReplaceHardwareDevice> oldDevices, List<ReplaceHardwareDevice> newDevices)
		: base(oldDevices, newDevices)
	{
		this.locals = locals;
	}

	protected override void AddDevice(ReplaceHardwareDevice device)
	{
		DeviceSetupInformation deviceSetupInformation = locals.NewHardwareSetupInformation.Devices.First((DeviceSetupInformation d) => d.DeviceHwid == device.Hwid.Text);
		DeviceSetupSettings deviceSetupSettings = locals.NewHardwareInfo.Devices.First((DeviceSetupSettings d) => d.DeviceHwid == device.Hwid);
		string text = NamingScheme.FindDeviceName(DeviceNameTemplate.Default, locals.NewHardwareInfo, deviceSetupSettings);
		locals.InheritedDevices.Add(new RHInheritedDevice(deviceSetupInformation.DeviceHwid, text, text, deviceSetupInformation.SettingsXml, deviceSetupInformation.PresetsXml, Guid.Empty, transferDevicePresets: false));
	}

	protected override void RemoveDevice(ReplaceHardwareDevice device)
	{
		locals.RemovedDevices.Add(device.Id);
	}

	protected override void ReplaceDevice(ReplaceHardwareDevice oldDevice, ReplaceHardwareDevice newDevice)
	{
		DeviceSetupInformation deviceSetupInformation = locals.NewHardwareSetupInformation.Devices.First((DeviceSetupInformation d) => d.DeviceHwid == newDevice.Hwid.Text);
		DeviceSetupSettings deviceSetupSettings = locals.NewHardwareInfo.Devices.First((DeviceSetupSettings d) => d.DeviceHwid == newDevice.Hwid);
		string text = NamingScheme.FindDeviceName(DeviceNameTemplate.Default, locals.NewHardwareInfo, deviceSetupSettings);
		locals.InheritedDevices.Add(new RHInheritedDevice(deviceSetupInformation.DeviceHwid, text, text, deviceSetupInformation.SettingsXml, deviceSetupInformation.PresetsXml, oldDevice.Id, transferDevicePresets: true));
	}
}
