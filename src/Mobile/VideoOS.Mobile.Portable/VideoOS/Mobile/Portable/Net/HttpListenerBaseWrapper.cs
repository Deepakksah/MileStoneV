using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.Portable.Net;

public class HttpListenerBaseWrapper : HttpListenerBase
{
	public HttpListenerBaseWrapper()
	{
	}

	public HttpListenerBaseWrapper(IEnumerable<string> prefixes)
		: base(prefixes)
	{
	}

	public static string GetAccessControlAllowOriginValue(HttpListenerContext httpContext, IList<string> allowedOrigins, out string requestOrigin)
	{
		string result = string.Empty;
		string origin = httpContext.Request.Headers.AllKeys.FirstOrDefault((string k) => string.Equals(k, "Origin", StringComparison.CurrentCultureIgnoreCase));
		requestOrigin = origin;
		if (allowedOrigins.Contains("*"))
		{
			result = "*";
		}
		else if (!string.IsNullOrWhiteSpace(origin) && (allowedOrigins.Any((string head) => httpContext.Request.Headers[origin].Contains(head)) || allowedOrigins.Contains(httpContext.Request.Headers[origin])))
		{
			result = httpContext.Request.Headers[origin];
		}
		return result;
	}

	public string GetAccessControlAllowOriginValue(HttpListenerContext httpContext)
	{
		string requestOrigin;
		return GetAccessControlAllowOriginValue(httpContext, HttpConfiguration.AccessControlAllowOrigin, out requestOrigin);
	}

	protected override void OnNewHttpClient(HttpListenerContext httpContext)
	{
		string accessControlAllowOriginValue = GetAccessControlAllowOriginValue(httpContext);
		if (!string.IsNullOrWhiteSpace(accessControlAllowOriginValue))
		{
			httpContext.Response.AddHeaderExt("Access-Control-Allow-Origin", accessControlAllowOriginValue);
			if (httpContext.Request.HttpMethod == HttpMethod.Options.Method)
			{
				AddAccessControlAllowMethodsAndHeaders(httpContext);
			}
			AddVaryHeader(httpContext);
		}
		httpContext.Response.AddHeaderExt("X-Frame-Options", HttpConfiguration.XFrameOptions);
		httpContext.Response.AddHeaderExt("Content-Security-Policy", HttpConfiguration.ContentSecurityPolicyProvider(httpContext));
		httpContext.Response.AddHeaderExt("X-XSS-Protection", HttpConfiguration.XXssProtection);
		httpContext.Response.AddHeaderExt("Strict-Transport-Security", HttpConfiguration.StrictTransportSecurity);
		httpContext.Response.AddHeaderExt("X-Content-Type-Options", "nosniff");
		httpContext.Response.RemoveHeaderExt("Server");
		HttpMethods key = (HttpMethods)Enum.Parse(typeof(HttpMethods), httpContext.Request.HttpMethod);
		if (base.SubscribedMethods?[key] != null)
		{
			base.SubscribedMethods[key](httpContext);
			return;
		}
		if (httpContext.Request.HttpMethod != HttpMethod.Options.Method)
		{
			httpContext.Response.StatusCode = 501;
		}
		httpContext.Response.Close();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AddAccessControlAllowMethodsAndHeaders(HttpListenerContext httpContext)
	{
		httpContext.Response.AddHeaderExt("Access-Control-Allow-Methods", HttpConfiguration.AccessControlAllowMethods);
		httpContext.Response.AddHeaderExt("Access-Control-Allow-Headers", HttpConfiguration.AccessControlAllowHeaders);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void AddVaryHeader(HttpListenerContext httpContext)
	{
		if (!HttpConfiguration.AccessControlAllowOrigin.Contains("*") && HttpConfiguration.AccessControlAllowOrigin.Count > 1)
		{
			httpContext.Response.AddHeaderExt("Vary", "Origin");
		}
	}
}
