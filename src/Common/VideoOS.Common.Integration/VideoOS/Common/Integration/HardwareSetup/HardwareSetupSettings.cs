using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.DeviceDiscovery;

namespace VideoOS.Common.Integration.HardwareSetup;

public class HardwareSetupSettings
{
	public MainDefinition Definition { get; set; }

	public Guid DriverId { get; private set; }

	public int DriverNumber { get; private set; }

	public string ProductId { get; private set; }

	public Uri Address { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public string DefinitionXml { get; private set; }

	public string SettingsXml { get; private set; }

	public string CustomDriverData { get; private set; }

	public HardwareInfo HostedHardware { get; private set; }

	public bool IsInterconnectedProduct => DriverNumbering.IsIcpDriver(DriverNumber);

	public bool IsVpsProduct => DriverNumbering.IsVpsDriver(DriverNumber);

	public bool IsMipDriver => DriverNumbering.IsMipDriver(DriverNumber);

	public bool IsArculesMipDriver => DriverNumbering.IsArculesMipDriver(DriverNumber);

	public bool IsIcpOrMipDriver
	{
		get
		{
			if (!DriverNumbering.IsIcpDriver(DriverNumber))
			{
				return DriverNumbering.IsMipDriver(DriverNumber);
			}
			return true;
		}
	}

	public string Model
	{
		get
		{
			if (IsInterconnectedProduct)
			{
				return DefinitionUtility.GetCapabilityDisplayValue(Definition.HardwareDefinition, new DriverSettings(SettingsXml), TypeReferenceIds.ProductName);
			}
			string text = ExtractDetectedModelNameTypeFromSettings(SettingsXml);
			if (text != null)
			{
				return text;
			}
			return Definition.HardwareDefinition.ProductIdDisplayNameResolved();
		}
	}

	public string InterconnectedHardwareName
	{
		get
		{
			if (!IsIcpOrMipDriver)
			{
				return string.Empty;
			}
			return DefinitionUtility.GetCapabilityDisplayValue(Definition.HardwareDefinition, new DriverSettings(SettingsXml), TypeReferenceIds.HardwareName);
		}
	}

	public string Name { get; set; }

	public bool Enabled { get; set; }

	public DeviceSetupInfoCollection Devices { get; private set; }

	public static string ExtractDetectedModelNameTypeFromSettings(string settingsXml)
	{
		string valueByTypeId = new DriverSettings(settingsXml).GetValueByTypeId("hardware", "928B6039-4E4A-4B03-9A82-A1CF29748ACC");
		if (string.IsNullOrEmpty(valueByTypeId))
		{
			return null;
		}
		return valueByTypeId;
	}

	public HardwareSetupSettings(Guid driverId, int driverNumber, string productId, Uri address, string userName, string password, string definitionXml, string settingsXml, string customDriverData, HardwareInfo hostedHardware)
	{
		DriverId = driverId;
		DriverNumber = driverNumber;
		ProductId = productId;
		Address = address;
		UserName = userName;
		Password = password;
		DefinitionXml = definitionXml;
		SettingsXml = settingsXml;
		CustomDriverData = customDriverData;
		HostedHardware = hostedHardware;
		Name = string.Empty;
		Enabled = false;
		Devices = new DeviceSetupInfoCollection();
	}

	public static HardwareSetupSettings CreateFromHardwareSetupInfo(HardwareSetupInformation hardwareSetupInformation, Uri address, string userName, string password, string customDriverData, HardwareInfo hostedHardware)
	{
		if (hardwareSetupInformation == null)
		{
			throw new ArgumentNullException("hardwareSetupInformation");
		}
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		HardwareSetupSettings hardwareSetupSettings = new HardwareSetupSettings(hardwareSetupInformation.DriverId, hardwareSetupInformation.DriverNumber, hardwareSetupInformation.ProductId, address, userName, password, hardwareSetupInformation.DefinitionXml, hardwareSetupInformation.SettingsXml, customDriverData, hostedHardware);
		List<string> list = new List<string>();
		list.Add(hardwareSetupInformation.SettingsXml);
		list.AddRange(hardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => d.SettingsXml));
		hardwareSetupSettings.Definition = MainDefinition.LoadXml(hardwareSetupInformation.DefinitionXml, list);
		foreach (DeviceSetupSettings item in hardwareSetupInformation.Devices.Select((DeviceSetupInformation d) => new DeviceSetupSettings(hardwareSetupSettings, new DeviceHwid(d.DeviceHwid), d.DeviceType, d.SettingsXml, d.PresetsXml, d.Streams)).OrderBy((DeviceSetupSettings d) => d.DeviceHwid, new DeviceHwidComparer()))
		{
			hardwareSetupSettings.Devices.Add(item);
		}
		return hardwareSetupSettings;
	}
}
