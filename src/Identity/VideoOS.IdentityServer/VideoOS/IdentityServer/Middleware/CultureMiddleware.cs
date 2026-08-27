using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace VideoOS.IdentityServer.Middleware;

public class CultureMiddleware
{
	private readonly RequestDelegate _next;

	private readonly QueryStringRequestCultureProvider _queryStringRequestCultureProvider;

	private readonly CookieRequestCultureProvider _cookieRequestCultureProvider;

	public CultureMiddleware(RequestDelegate next, IOptions<RequestLocalizationOptions> requestLocalizationOptions)
	{
		_next = next;
		_queryStringRequestCultureProvider = requestLocalizationOptions.Value.RequestCultureProviders.OfType<QueryStringRequestCultureProvider>().FirstOrDefault();
		_cookieRequestCultureProvider = requestLocalizationOptions.Value.RequestCultureProviders.OfType<CookieRequestCultureProvider>().FirstOrDefault();
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (_queryStringRequestCultureProvider != null && _cookieRequestCultureProvider != null)
		{
			string culture = (await _queryStringRequestCultureProvider.DetermineProviderCultureResult(context))?.Cultures.FirstOrDefault().Value;
			if (culture != null)
			{
				culture = (CultureInfo.GetCultures(CultureTypes.AllCultures).Any((CultureInfo c) => string.Equals(c.Name, culture, StringComparison.CurrentCultureIgnoreCase)) ? culture : "en-US");
				context.Response.Cookies.Append(_cookieRequestCultureProvider.CookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));
			}
		}
		await _next(context);
	}
}
