using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Server.CommandHandler;

[DebuggerDisplay("ReplaceHardwareDevice {Hwid.Text}")]
public class ReplaceHardwareDevice
{
	public DeviceHwid Hwid { get; }

	public bool Enabled { get; set; }

	public Guid Id { get; }

	public ReplaceHardwareDevice(DeviceHwid hwid, bool enabled, Guid id)
	{
		Hwid = hwid;
		Enabled = enabled;
		Id = id;
	}

	public ReplaceHardwareDevice(DeviceSetupInformation device, Guid id)
	{
		Hwid = new DeviceHwid(device.DeviceHwid);
		Enabled = true;
		Id = id;
	}

	public ReplaceHardwareDevice(Device device)
	{
		Hwid = device.Hwid;
		Enabled = device.Enabled;
		Id = device.ID;
	}
}
