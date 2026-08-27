using System.Drawing;

namespace VideoOS.Mobile.Oem;

public interface ICoBrandingManager
{
	bool CoBrandingEnabled { get; }

	byte[] LogoLarge { get; }

	byte[] LogoSmall { get; }

	byte[] LogoWeb { get; }

	Image LogoLargeImage { get; }

	Image LogoSmallImage { get; }

	Image LogoWebImage { get; }

	string LogoUrl { get; }

	string ServerName { get; }
}
