using System.Drawing;
using VideoOS.Utilities.Colors;
using VideoOS.Utilities.OemResource;

namespace VideoOS.IdentityServer.Oem;

public interface IOemResources
{
	Color PageBackgroundColor { get; }

	VideoOS.Utilities.Colors.ColorConverter.HSL PrimaryButtonColor { get; }

	string SiteFavIcon { get; }

	VectorGraphicsResource FormHeaderImage { get; }

	string IdpReturnUrl { get; }
}
