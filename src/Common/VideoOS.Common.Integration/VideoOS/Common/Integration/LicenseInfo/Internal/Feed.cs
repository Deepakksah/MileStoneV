using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class Feed : ILicenseFeed
{
	public string Name { get; private set; }

	public int Count { get; set; }

	public Feed(string name, int count)
	{
		Name = name;
		Count = count;
	}
}
