using Microsoft.Extensions.Localization;
using VideoOS.IdentityServer.Resources;

namespace VideoOS.IdentityServer.Localization;

public class StringLocalizer
{
	private readonly IStringLocalizer _localizer;

	public StringLocalizer(IStringLocalizerFactory factory)
	{
		_localizer = factory.Create(typeof(IdentityResource));
	}

	public LocalizedString GetString(string key)
	{
		return _localizer[key];
	}

	public LocalizedString GetString(string key, string parameter)
	{
		return _localizer[key, new object[1] { parameter }];
	}
}
