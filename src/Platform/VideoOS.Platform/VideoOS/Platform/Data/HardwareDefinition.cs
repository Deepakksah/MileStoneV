using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using VideoOS.Platform.Data.HardwareDeviceDefinitions;

namespace VideoOS.Platform.Data;

public class HardwareDefinition
{
	private readonly PhysicalAddress _macAddress;

	private readonly string _model;

	private readonly List<MetadataDeviceDefintion> _metadataDevices = new List<MetadataDeviceDefintion>();

	private readonly List<CameraDeviceDefinition> _cameraDevices = new List<CameraDeviceDefinition>();

	private readonly List<MicrophoneDeviceDefinition> _micDevices = new List<MicrophoneDeviceDefinition>();

	public PhysicalAddress MacAddress => _macAddress;

	public string Model => _model;

	public string Firmware { get; set; }

	public string SerialNr { get; set; }

	public List<MetadataDeviceDefintion> MetadataDevices => _metadataDevices;

	public List<CameraDeviceDefinition> CameraDevices => _cameraDevices;

	public List<MicrophoneDeviceDefinition> MicrophoneDevices => _micDevices;

	public HardwareDefinition(PhysicalAddress macAddress, string model)
	{
		if (macAddress == null)
		{
			throw new ArgumentNullException("macAddress");
		}
		if (model == null)
		{
			throw new ArgumentNullException("model");
		}
		if (string.IsNullOrWhiteSpace(model))
		{
			throw new ArgumentException("model");
		}
		_macAddress = macAddress;
		_model = model;
	}

	public IDictionary<string, string> GenerateParameters()
	{
		return GenerateParameters(null);
	}

	public IDictionary<string, string> GenerateParameters(MediaProviderService mediaProviderService)
	{
		if (_cameraDevices.Count > 1)
		{
			throw new InvalidOperationException("The MIP driver currently does not support more than one camera device per hardware device");
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{
				"Mac",
				_macAddress.ToString()
			},
			{ "Model", _model },
			{
				"Firmware",
				Firmware ?? "NotSupported"
			},
			{
				"SerialNr",
				SerialNr ?? "NotSupported"
			},
			{
				"MDStreamCount",
				_metadataDevices.Count.ToString(CultureInfo.InvariantCulture)
			},
			{
				"NumVideoCh",
				_cameraDevices.Count.ToString(CultureInfo.InvariantCulture)
			},
			{
				"MicStreamCount",
				_micDevices.Count.ToString(CultureInfo.InvariantCulture)
			}
		};
		if (_cameraDevices.Count == 1)
		{
			_cameraDevices[0].GenerateDeviceParameters(dictionary);
		}
		for (int i = 0; i < _micDevices.Count; i++)
		{
			string prefix = "MicStream." + i.ToString(CultureInfo.InvariantCulture) + ".";
			_micDevices[i].GenerateDeviceParameters(prefix, dictionary);
		}
		for (int j = 0; j < _metadataDevices.Count; j++)
		{
			string devicePrefix = "MDStream." + j.ToString(CultureInfo.InvariantCulture);
			IDictionary<string, string> currentValues = null;
			if (mediaProviderService != null)
			{
				MetadataProviderChannel metadataProviderChannel = mediaProviderService.GetMetadataProviderChannel(j + 1);
				if (metadataProviderChannel != null)
				{
					currentValues = metadataProviderChannel.ParameterDictionary;
				}
			}
			_metadataDevices[j].GenerateDeviceParametersWithCurrent(devicePrefix, dictionary, currentValues);
		}
		return dictionary;
	}
}
