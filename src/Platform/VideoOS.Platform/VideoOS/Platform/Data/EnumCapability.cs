using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Data;

public sealed class EnumCapability : MetadataDeviceCapability
{
	private readonly List<EnumCapabilityValue> _enumValues = new List<EnumCapabilityValue>();

	public List<EnumCapabilityValue> EnumValues => _enumValues;

	public string DefaultValue { get; set; }

	public override string CapabilityType => "Enum";

	public override void WriteSubclassParameters(string capabilityPrefix, IDictionary<string, string> parameters)
	{
		parameters.Add(capabilityPrefix + ".Default", DefaultValue);
		parameters.Add(capabilityPrefix + ".Count", _enumValues.Count.ToString(CultureInfo.InvariantCulture));
		for (int i = 0; i < _enumValues.Count; i++)
		{
			string valuePrefix = capabilityPrefix + "." + i.ToString(CultureInfo.InvariantCulture);
			_enumValues[i].GenerateEnumValueParameters(valuePrefix, parameters);
		}
	}
}
