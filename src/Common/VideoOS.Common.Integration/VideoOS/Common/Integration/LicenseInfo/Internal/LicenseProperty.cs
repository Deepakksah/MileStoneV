using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class LicenseProperty : ILicenseProperty
{
	public string PropertyType { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public LicenseProperty(string propertyType, string name, string value)
	{
		PropertyType = propertyType;
		Name = name;
		Value = value;
	}
}
