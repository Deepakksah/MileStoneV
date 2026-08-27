using System.Drawing;
using VideoOS.Utilities.Colors;
using VideoOS.Utilities.OemResource;

namespace VideoOS.IdentityServer.Oem;

public class OemResourcesProvider : IOemResources
{
	public Color PageBackgroundColor => OemResources.Instance.ColorRGB("BackgroundColorR", "BackgroundColorG", "BackgroundColorB");

	public VideoOS.Utilities.Colors.ColorConverter.HSL PrimaryButtonColor => new VideoOS.Utilities.Colors.ColorConverter.HSL(OemResources.Instance.GetResourceString("Primary_Color"));

	public string SiteFavIcon => OemResources.Instance.GetResourceBase64VectorGraphics("SiteFavIcon");

	public VectorGraphicsResource FormHeaderImage => OemResources.Instance.GetResourceVectorGraphics("CompanyLogo");

	public string IdpReturnUrl => OemResources.Instance.GetResourceString("MobileClientIdpReturnUrl");
}
