using System.Diagnostics;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Common;

[DebuggerDisplay("PackageParameter: {Target} -> {Value}")]
public class PackageParameter : IPackageParameter
{
	public string Target { get; set; }

	public string Value { get; set; }

	public PackageParameter(string target, string value)
	{
		Target = target;
		Value = value;
	}
}
