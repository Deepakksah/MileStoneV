using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public class EnumCapabilityValue
{
	public string Name { get; set; }

	public Guid Id { get; set; }

	public string DisplayName { get; set; }

	public Guid DisplayId { get; set; }

	public string Value { get; set; }

	public void GenerateEnumValueParameters(string valuePrefix, IDictionary<string, string> parameters)
	{
		parameters.Add(valuePrefix + ".Name", Name);
		parameters.Add(valuePrefix + ".GUID", Id.ToString());
		parameters.Add(valuePrefix + ".Disp.Name", DisplayName);
		parameters.Add(valuePrefix + ".Disp.GUID", DisplayId.ToString());
		parameters.Add(valuePrefix + ".Value", Value);
	}
}
