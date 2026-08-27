using System.Diagnostics;

namespace VideoOS.Common.Integration.LicenseInfo;

[DebuggerDisplay("FeatureParameterInformation: {Name}, {Value}")]
public class FeatureParameterInformation
{
	public string Name { get; private set; }

	public string DisplayName { get; private set; }

	public string Value { get; private set; }

	public FeatureParameterInformation(string name, string displayName, string value)
	{
		Name = name;
		DisplayName = displayName;
		Value = value;
	}
}
