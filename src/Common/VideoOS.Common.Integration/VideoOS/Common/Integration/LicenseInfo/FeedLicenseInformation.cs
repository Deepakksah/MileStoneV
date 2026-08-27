using System.Diagnostics;

namespace VideoOS.Common.Integration.LicenseInfo;

[DebuggerDisplay("FeedInformation: {TypeName}")]
public class FeedLicenseInformation
{
	public string TypeName { get; private set; }

	public int Available { get; set; }

	public int Used { get; set; }

	public int Temporary { get; set; }

	public int Expired { get; set; }

	public int Missing { get; set; }

	public FeedLicenseInformation(string typeName)
	{
		TypeName = typeName;
	}
}
