using System.Globalization;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

public class MapUpdateData
{
	public string Id { get; set; }

	public string Name { get; set; }

	public ServerId ServerId { get; set; }

	public int ServerOrder { get; set; }

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}", Name);
	}
}
