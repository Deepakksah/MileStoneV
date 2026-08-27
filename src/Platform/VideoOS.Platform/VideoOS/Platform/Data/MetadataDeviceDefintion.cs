using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Data;

public class MetadataDeviceDefintion
{
	private readonly List<MetadataTypeDefinition> _metadataTypes = new List<MetadataTypeDefinition>();

	public List<MetadataTypeDefinition> MetadataTypes => _metadataTypes;

	public static MetadataDeviceDefintion CreateGpsDevice()
	{
		return new MetadataDeviceDefintion
		{
			MetadataTypes = { MetadataTypeDefinition.CreateGpsDefinition() }
		};
	}

	public static MetadataDeviceDefintion CreateBoundingBoxDevice()
	{
		return new MetadataDeviceDefintion
		{
			MetadataTypes = { MetadataTypeDefinition.CreateBoundingBoxDefinition() }
		};
	}

	public void GenerateDeviceParameters(string devicePrefix, IDictionary<string, string> parameters)
	{
		GenerateDeviceParametersWithCurrent(devicePrefix, parameters, null);
	}

	public void GenerateDeviceParametersWithCurrent(string devicePrefix, IDictionary<string, string> parameters, IDictionary<string, string> currentValues)
	{
		parameters.Add(devicePrefix + ".Count", _metadataTypes.Count.ToString(CultureInfo.InvariantCulture));
		for (int i = 0; i < _metadataTypes.Count; i++)
		{
			string typePrefix = devicePrefix + "." + i.ToString(CultureInfo.InvariantCulture);
			_metadataTypes[i].GenerateTypeDefinitionParametersWithCurrent(typePrefix, parameters, currentValues);
		}
	}
}
