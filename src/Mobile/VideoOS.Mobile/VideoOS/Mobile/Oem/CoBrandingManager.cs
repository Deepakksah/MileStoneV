using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using VideoOS.Mobile.VideoSource;

namespace VideoOS.Mobile.Oem;

public class CoBrandingManager : ICoBrandingManager
{
	public static CoBrandingManager Instance = new CoBrandingManager();

	public const string LogoLargeLink = "Branding/Cobranding/LogoLarge";

	public const string LogoSmallLink = "Branding/Cobranding/LogoSmall";

	public const string LogoWebLink = "Branding/Cobranding/LogoWeb";

	private bool _coBrandingEnabled;

	private byte[] _logoLarge;

	private byte[] _logoSmall;

	private byte[] _logoWeb;

	private Image _logoLargeImage;

	private Image _logoSmallImage;

	private Image _logoWebImage;

	private string _logoUrl;

	private string _serverName;

	public bool CoBrandingEnabled => _coBrandingEnabled;

	public byte[] LogoLarge => GetBytes(_logoLargeImage, ref _logoLarge);

	public byte[] LogoSmall => GetBytes(_logoSmallImage, ref _logoSmall);

	public byte[] LogoWeb => GetBytes(_logoWebImage, ref _logoWeb);

	public Image LogoLargeImage => _logoLargeImage;

	public Image LogoSmallImage => _logoSmallImage;

	public Image LogoWebImage => _logoWebImage;

	public string LogoUrl => _logoUrl;

	public string ServerName => _serverName;

	private CoBrandingManager()
	{
		_coBrandingEnabled = false;
		_logoLarge = null;
		_logoSmall = null;
		_logoWeb = null;
		_logoLargeImage = null;
		_logoSmallImage = null;
		_logoWebImage = null;
		_logoUrl = "";
		_serverName = "";
	}

	public byte[] GetBytes(Image logo, ref byte[] logoBytes)
	{
		if (logoBytes == null && logo != null)
		{
			logoBytes = ImageUtils.ImageToByteArr(logo, ImageFormat.Png);
		}
		return logoBytes;
	}

	public void GetCoBrandingOutParams(Dictionary<string, string> output)
	{
		output["CoBranding"] = (CoBrandingEnabled ? "Yes" : "No");
		if (CoBrandingEnabled)
		{
			output["CoBrandingLogoLarge"] = "Branding/Cobranding/LogoLarge";
			output["CoBrandingLogoSmall"] = "Branding/Cobranding/LogoSmall";
			output["CoBrandingLogoWeb"] = "Branding/Cobranding/LogoWeb";
			output["CoBrandingLink"] = LogoUrl;
			output["ServerDescription"] = ServerName;
		}
	}

	public void InitFromEnvironmentSdk()
	{
		InitString("CobrandLink", ref _logoUrl);
		InitString("ServerDescription", ref _serverName);
		InitLogo("LogoLarge", out _logoLargeImage, out _logoLarge);
		InitLogo("LogoSmall", out _logoSmallImage, out _logoSmall);
		InitLogo("LogoWeb", out _logoWebImage, out _logoWeb);
		_coBrandingEnabled = CheckAllFieldsAreSet();
	}

	private void InitString(string key, ref string text)
	{
		text = MipSdkWrapperOem.GetOemStringSingleLine(key);
		if (string.IsNullOrWhiteSpace(text))
		{
			_coBrandingEnabled = false;
		}
	}

	private void InitLogo(string key, out Image logo, out byte[] logoBytes)
	{
		logo = MipSdkWrapperOem.GetOemImage(key);
		if (logo == null)
		{
			_coBrandingEnabled = false;
		}
		logoBytes = null;
	}

	public bool CheckAllFieldsAreSet()
	{
		if (!string.IsNullOrWhiteSpace(_logoUrl) && !string.IsNullOrWhiteSpace(_serverName) && _logoLargeImage != null && _logoSmallImage != null)
		{
			return _logoWebImage != null;
		}
		return false;
	}

	public bool CheckAtLeastOneFieldsIsSet()
	{
		if (string.IsNullOrWhiteSpace(_logoUrl) && string.IsNullOrWhiteSpace(_serverName) && _logoLargeImage == null && _logoSmallImage == null)
		{
			return _logoWebImage != null;
		}
		return true;
	}
}
