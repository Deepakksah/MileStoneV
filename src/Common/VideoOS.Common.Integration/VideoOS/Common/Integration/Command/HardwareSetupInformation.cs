using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class HardwareSetupInformation : ISerializable
{
	public Guid DriverId { get; private set; }

	public string DefinitionXml { get; set; }

	public string SettingsXml { get; private set; }

	public string ProductId { get; private set; }

	public int DriverNumber { get; private set; }

	public DeviceSetupInformationCollection Devices { get; private set; }

	public HardwareSetupInformation(Guid driverId, string definitionXml, string settingsXml, string productId, int driverNumber)
	{
		DriverId = driverId;
		DefinitionXml = definitionXml;
		SettingsXml = settingsXml;
		ProductId = productId;
		DriverNumber = driverNumber;
		Devices = new DeviceSetupInformationCollection();
	}

	protected HardwareSetupInformation(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		DriverId = new Guid(info.GetString("DriverId"));
		DefinitionXml = info.GetString("DefinitionXml");
		SettingsXml = info.GetString("SettingsXml");
		ProductId = info.GetString("ProductId");
		DriverNumber = info.GetInt32("DriverNumber");
		BinaryFormatter binaryFormatter = new BinaryFormatter().Wrap(typeof(DeviceSetupInformationCollection), typeof(DeviceSetupInformation));
		MemoryStream memoryStream = new MemoryStream((byte[])info.GetValue("Devices", typeof(byte[])));
		Devices = (DeviceSetupInformationCollection)binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("DriverId", DriverId.ToString());
		info.AddValue("DefinitionXml", DefinitionXml);
		info.AddValue("SettingsXml", SettingsXml);
		info.AddValue("ProductId", ProductId);
		info.AddValue("DriverNumber", DriverNumber);
		MemoryStream memoryStream = new MemoryStream();
		new BinaryFormatter().Serialize(memoryStream, Devices);
		byte[] value = memoryStream.ToArray();
		memoryStream.Close();
		info.AddValue("Devices", value);
	}
}
