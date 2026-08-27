using System.Globalization;

namespace VideoOS.Management.VmoClient;

public class VmoClientEnvironment
{
	public CultureInfo CurrentUICulture { get; set; }

	public CultureInfo ResolveUICulture()
	{
		if (CurrentUICulture != null)
		{
			return CurrentUICulture;
		}
		return CultureInfo.CurrentUICulture;
	}
}
