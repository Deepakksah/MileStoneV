using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class Port : IPort
{
	public string Name { get; private set; }

	public int Count { get; set; }

	public Port(string name, int count)
	{
		Name = name;
		Count = count;
	}
}
