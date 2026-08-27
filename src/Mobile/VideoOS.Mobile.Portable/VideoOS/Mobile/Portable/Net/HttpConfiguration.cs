using System;
using System.Collections.Generic;
using System.Net;

namespace VideoOS.Mobile.Portable.Net;

public class HttpConfiguration
{
	public static List<string> AccessControlAllowOrigin;

	public static string AccessControlAllowMethods;

	public static string AccessControlAllowHeaders;

	public static Func<HttpListenerContext, string> ContentSecurityPolicyProvider;

	public static string XFrameOptions;

	public static string XXssProtection;

	public static string StrictTransportSecurity;

	static HttpConfiguration()
	{
		AccessControlAllowOrigin = new List<string>();
		ContentSecurityPolicyProvider = (HttpListenerContext httpHeader) => "default-src 'self'; script-src 'self'; connect-src 'self'; img-src 'self'; style - src 'self'; frame - ancestors 'self'; ";
		AccessControlAllowHeaders = "Content-Type";
		AccessControlAllowMethods = "POST, GET, OPTIONS";
		XFrameOptions = "DENY";
		XXssProtection = "1; mode=block";
		StrictTransportSecurity = "max-age=31536000";
	}
}
