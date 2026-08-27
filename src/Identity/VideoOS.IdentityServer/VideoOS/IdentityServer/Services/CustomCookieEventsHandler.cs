using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Helper;

namespace VideoOS.IdentityServer.Services;

public class CustomCookieEventsHandler(IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor) : CookieAuthenticationEvents
{
	public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
	{
		CookieAuthenticationOptions cookieAuthenticationOptions = optionsMonitor.Get(context.Scheme.Name);
		HttpRequest request = context.HttpContext.Request;
		if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) || request.Query.ContainsKey(cookieAuthenticationOptions.ReturnUrlParameter))
		{
			context.HttpContext.Session.SetSessionIntValue("ExpireSession", 1);
		}
		string text = context.Request.Query[cookieAuthenticationOptions.ReturnUrlParameter].ToString();
		if (!string.IsNullOrEmpty(text) && Uri.IsWellFormedUriString(text, UriKind.Relative))
		{
			string text2 = (cookieAuthenticationOptions.LoginPath.HasValue ? request.PathBase.Add(cookieAuthenticationOptions.LoginPath).Value : "/Identity/Account/Login");
			if (string.IsNullOrEmpty(text2))
			{
				text2 = "/Identity/Account/Login";
			}
			string location = QueryHelpers.AddQueryString(text2, cookieAuthenticationOptions.ReturnUrlParameter, text);
			context.Response.Redirect(location);
			return Task.CompletedTask;
		}
		return base.RedirectToLogin(context);
	}
}
