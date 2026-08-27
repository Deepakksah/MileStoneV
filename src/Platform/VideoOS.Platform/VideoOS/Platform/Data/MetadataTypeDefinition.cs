using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Data;

public class MetadataTypeDefinition
{
	private readonly List<MetadataDeviceCapability> _capabilities = new List<MetadataDeviceCapability>();

	public List<MetadataDeviceCapability> Capabilities => _capabilities;

	public string Name { get; set; }

	public Guid Id { get; set; }

	public string DisplayName { get; set; }

	public Guid DisplayId { get; set; }

	public static MetadataTypeDefinition CreateGpsDefinition()
	{
		return new MetadataTypeDefinition
		{
			Name = "META_DATA_GPS",
			Id = new Guid("E634BF72-94F0-497B-A63D-9363AAD3CAB7"),
			DisplayName = "Metadata GPS",
			DisplayId = new Guid("2DD43C94-372C-49C6-B834-15A4AC723C6C"),
			Capabilities = { (MetadataDeviceCapability)DataCapability.CreateValidTimeCapability() }
		};
	}

	public static MetadataTypeDefinition CreateBoundingBoxDefinition()
	{
		return new MetadataTypeDefinition
		{
			Name = "META_DATA_BOUNDING_BOX",
			Id = new Guid("8BEA15BB-2EB6-4BC3-BDDA-D1773374ABAD"),
			DisplayName = "Metadata bounding box",
			DisplayId = new Guid("AA02034C-132B-421D-A62E-F3C05808A417"),
			Capabilities = { (MetadataDeviceCapability)DataCapability.CreateValidTimeCapability() }
		};
	}

	public void GenerateTypeDefinitionParameters(string typePrefix, IDictionary<string, string> parameters)
	{
		GenerateTypeDefinitionParametersWithCurrent(typePrefix, parameters, null);
	}

	public void GenerateTypeDefinitionParametersWithCurrent(string typePrefix, IDictionary<string, string> parameters, IDictionary<string, string> currentValues)
	{
		parameters.Add(typePrefix + ".Name", Name);
		parameters.Add(typePrefix + ".GUID", Id.ToString());
		parameters.Add(typePrefix + ".Disp.Name", DisplayName);
		parameters.Add(typePrefix + ".Disp.GUID", DisplayId.ToString());
		parameters.Add(typePrefix + ".Cap.Count", _capabilities.Count.ToString(CultureInfo.InvariantCulture));
		for (int i = 0; i < _capabilities.Count; i++)
		{
			string capabilityPrefix = typePrefix + "." + i.ToString(CultureInfo.InvariantCulture);
			_capabilities[i].GenerateDeviceCapabilityParametersWithCurrent(capabilityPrefix, parameters, currentValues);
		}
	}
}
