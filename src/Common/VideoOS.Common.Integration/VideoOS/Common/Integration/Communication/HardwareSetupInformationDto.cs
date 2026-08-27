using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class HardwareSetupInformationDto
{
	public Guid DriverId { get; private set; }

	public string DefinitionXml { get; private set; }

	public string SettingsXml { get; private set; }

	public string ProductId { get; private set; }

	public int DriverNumber { get; private set; }

	public DeviceSetupInformationCollectionDto DeviceSetupInformations { get; private set; }

	public static HardwareSetupInformationDto Create(HardwareSetupInformation hardwareSetupInformation)
	{
		if (hardwareSetupInformation == null)
		{
			throw new ArgumentNullException("hardwareSetupInformation");
		}
		HardwareSetupInformationDto hardwareSetupInformationDto = new HardwareSetupInformationDto();
		hardwareSetupInformationDto.DriverId = hardwareSetupInformation.DriverId;
		hardwareSetupInformationDto.DefinitionXml = hardwareSetupInformation.DefinitionXml;
		hardwareSetupInformationDto.SettingsXml = hardwareSetupInformation.SettingsXml;
		hardwareSetupInformationDto.ProductId = hardwareSetupInformation.ProductId;
		hardwareSetupInformationDto.DriverNumber = hardwareSetupInformation.DriverNumber;
		hardwareSetupInformationDto.DeviceSetupInformations = new DeviceSetupInformationCollectionDto();
		foreach (DeviceSetupInformation device in hardwareSetupInformation.Devices)
		{
			DeviceSetupInformationDto item = DeviceSetupInformationDto.Create(device);
			hardwareSetupInformationDto.DeviceSetupInformations.Add(item);
		}
		return hardwareSetupInformationDto;
	}

	public static HardwareSetupInformation CreateHardwareSetupInformation(HardwareSetupInformationDto hardwareSetupInformationDto)
	{
		if (hardwareSetupInformationDto == null)
		{
			throw new ArgumentNullException("hardwareSetupInformationDto");
		}
		HardwareSetupInformation hardwareSetupInformation = new HardwareSetupInformation(hardwareSetupInformationDto.DriverId, hardwareSetupInformationDto.DefinitionXml, hardwareSetupInformationDto.SettingsXml, hardwareSetupInformationDto.ProductId, hardwareSetupInformationDto.DriverNumber);
		foreach (DeviceSetupInformationDto deviceSetupInformation in hardwareSetupInformationDto.DeviceSetupInformations)
		{
			DeviceSetupInformation item = DeviceSetupInformationDto.CreateDeviceSetupInformation(deviceSetupInformation);
			hardwareSetupInformation.Devices.Add(item);
		}
		return hardwareSetupInformation;
	}
}
