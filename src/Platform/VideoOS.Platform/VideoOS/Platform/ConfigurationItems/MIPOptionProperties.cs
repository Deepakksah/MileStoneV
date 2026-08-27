using System;

namespace VideoOS.Platform.ConfigurationItems;

public class MIPOptionProperties
{
	public Guid OptionId { get; set; }

	public int DataVersion { get; set; }

	public string PropertiesXml { get; set; }
}
