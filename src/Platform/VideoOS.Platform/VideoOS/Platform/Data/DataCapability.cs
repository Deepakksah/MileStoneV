using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.Data;

public sealed class DataCapability : MetadataDeviceCapability
{
	public int Minimum { get; set; }

	public int Maximum { get; set; }

	public int Resolution { get; set; }

	public int Default { get; set; }

	public override string CapabilityType => "Data";

	public static DataCapability CreateValidTimeCapability()
	{
		return new DataCapability
		{
			Name = "ValidTime",
			TypeName = "ValidTimeType",
			TypeId = new Guid("ED0E8CFD-58BA-4334-8B47-111AA5D255C6"),
			TypeDisplayName = "Valid time (seconds)",
			TypeDisplayId = new Guid("8A829256-3594-4BA1-AEAA-8A0AFB5287F5"),
			Minimum = 1,
			Maximum = 3600,
			Resolution = 1,
			Default = 3
		};
	}

	public override void WriteSubclassParameters(string capabilityPrefix, IDictionary<string, string> parameters)
	{
		parameters.Add(capabilityPrefix + ".Minimum", Minimum.ToString(CultureInfo.InvariantCulture));
		parameters.Add(capabilityPrefix + ".Maximum", Maximum.ToString(CultureInfo.InvariantCulture));
		parameters.Add(capabilityPrefix + ".Resolution", Resolution.ToString(CultureInfo.InvariantCulture));
		parameters.Add(capabilityPrefix + ".Default", Default.ToString(CultureInfo.InvariantCulture));
	}
}
