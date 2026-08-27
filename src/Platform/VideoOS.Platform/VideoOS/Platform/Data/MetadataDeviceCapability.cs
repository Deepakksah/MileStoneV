using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public abstract class MetadataDeviceCapability
{
	public string Name { get; set; }

	public string TypeName { get; set; }

	public Guid TypeId { get; set; }

	public string TypeDisplayName { get; set; }

	public Guid TypeDisplayId { get; set; }

	public abstract string CapabilityType { get; }

	public void GenerateDeviceCapabilityParameters(string capabilityPrefix, IDictionary<string, string> parameters)
	{
		GenerateDeviceCapabilityParametersWithCurrent(capabilityPrefix, parameters, null);
	}

	public void GenerateDeviceCapabilityParametersWithCurrent(string capabilityPrefix, IDictionary<string, string> parameters, IDictionary<string, string> currentValues)
	{
		parameters.Add(capabilityPrefix + ".Type", CapabilityType);
		parameters.Add(capabilityPrefix + ".Cap.Name", Name);
		parameters.Add(capabilityPrefix + ".Type.Name", TypeName);
		parameters.Add(capabilityPrefix + ".Type.GUID", TypeId.ToString());
		parameters.Add(capabilityPrefix + ".Type.Disp.Name", TypeDisplayName);
		parameters.Add(capabilityPrefix + ".Type.Disp.GUID", TypeDisplayId.ToString());
		WriteSubclassParameters(capabilityPrefix, parameters);
		if (currentValues != null)
		{
			string value = null;
			if (currentValues.TryGetValue(Name, out value))
			{
				parameters.Add(capabilityPrefix + ".Value", value);
			}
		}
	}

	public abstract void WriteSubclassParameters(string capabilityPrefix, IDictionary<string, string> parameters);
}
