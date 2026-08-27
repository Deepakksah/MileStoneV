using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.HardwareSetup;

namespace VideoOS.Server.CommandHandler;

public class ReplaceHardwareLocals
{
	public HardwareSetupSettings NewHardwareInfo { get; }

	public string NewAuditHardwareName { get; }

	public HardwareSetupInformation NewHardwareSetupInformation { get; }

	public List<RHInheritedDevice> InheritedDevices { get; }

	public HashSet<Guid> RemovedDevices { get; }

	public ReplaceHardwareLocals(ReplaceHardwareTaskCommand replaceHardwareTaskCommand, Uri uri, HardwareSetupInformation hardwareSetup, IPHardware ipHardware)
	{
		NewHardwareSetupInformation = hardwareSetup;
		NewHardwareInfo = new HardwareSetupSettings(NewHardwareSetupInformation.DriverId, NewHardwareSetupInformation.DriverNumber, NewHardwareSetupInformation.ProductId, uri, replaceHardwareTaskCommand.UserName, replaceHardwareTaskCommand.Password, NewHardwareSetupInformation.DefinitionXml, NewHardwareSetupInformation.SettingsXml, replaceHardwareTaskCommand.CustomDriverData, null);
		List<string> list = new List<string> { NewHardwareSetupInformation.SettingsXml };
		list.AddRange(NewHardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => d.SettingsXml));
		NewHardwareInfo.Definition = MainDefinition.LoadXml(NewHardwareSetupInformation.DefinitionXml, list);
		NewAuditHardwareName = FormattableString.Invariant($"New hardware {NewHardwareInfo.Definition.Driver.Name} with address {ipHardware.Uri.DnsSafeHost}");
		IOrderedEnumerable<DeviceSetupSettings> orderedEnumerable = NewHardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => new DeviceSetupSettings(NewHardwareInfo, new DeviceHwid(d.DeviceHwid), d.DeviceType, d.SettingsXml, d.PresetsXml, d.Streams)).OrderBy((DeviceSetupSettings d) => d.DeviceHwid, new DeviceHwidComparer());
		foreach (DeviceSetupSettings item in orderedEnumerable)
		{
			NewHardwareInfo.Devices.Add(item);
		}
		NewHardwareInfo.Name = NamingScheme.FindHardwareName(HardwareNameTemplate.Default, NewHardwareInfo);
		InheritedDevices = new List<RHInheritedDevice>();
		RemovedDevices = new HashSet<Guid>();
	}
}
