using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Data;

public class MetadataHardwareDefinition
{
	private readonly List<MetadataDeviceDefintion> _metadataDevices = new List<MetadataDeviceDefintion>();

	public string MacAddress { get; set; }

	public string Firmware { get; set; }

	public string SerialNr { get; set; }

	public List<MetadataDeviceDefintion> MetadataDevices => _metadataDevices;

	public IDictionary<string, string> GenerateParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{
				"Mac",
				MacAddress ?? "NotSupported"
			},
			{
				"Firmware",
				Firmware ?? "NotSupported"
			},
			{
				"SerialNr",
				SerialNr ?? "NotSupported"
			},
			{ "Model", "MetadataProvider" },
			{ "Codec", "Metadata" },
			{ "CodecList", "Metadata" },
			{
				"MDStreamCount",
				MetadataDevices.Count.ToString(CultureInfo.InvariantCulture)
			}
		};
		for (int i = 0; i < _metadataDevices.Count; i++)
		{
			string devicePrefix = "MDStream." + i.ToString(CultureInfo.InvariantCulture);
			_metadataDevices[i].GenerateDeviceParameters(devicePrefix, dictionary);
		}
		return dictionary;
	}

	public static MetadataHardwareDefinition CreateGpsHardware()
	{
		return new MetadataHardwareDefinition
		{
			MetadataDevices = 
			{
				new MetadataDeviceDefintion
				{
					MetadataTypes = { MetadataTypeDefinition.CreateGpsDefinition() }
				}
			}
		};
	}
}
