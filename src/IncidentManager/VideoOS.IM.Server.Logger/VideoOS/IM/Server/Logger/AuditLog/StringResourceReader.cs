using System.Globalization;
using System.Resources;
using VideoOS.IM.Server.Localization.Resources;

namespace VideoOS.IM.Server.Logger.AuditLog;

public class StringResourceReader : IStringResourceReader
{
	private readonly ResourceManager _resource;

	public StringResourceReader()
	{
		_resource = new ResourceManager(typeof(Strings));
	}

	public string GetString(string name, CultureInfo culture)
	{
		return _resource.GetString(name, culture);
	}
}
