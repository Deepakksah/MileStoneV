using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

[DataContract]
public class HttpHeadersConfig : IConfigSection
{
	public const string NameContentSecurityPolicy = "Content-Security-Policy";

	public const string NameCSPAnalytics = "CSP-Analytics";

	public const string NameXFrameOptions = "X-Frame-Options";

	public const string NameAllowOrigins = "Access-Control-Allow-Origin";

	public const string NameAllowMethods = "Access-Control-Allow-Methods";

	public const string NameAllowHeaders = "Access-Control-Allow-Headers";

	public const string NameXXssProtection = "X-XSS-Protection";

	public const string NameStrictTransport = "Strict-Transport-Security";

	public const string NameConnectionId = "ConnectionId";

	[DataMember]
	public string ContentSecurityPolicy { get; set; }

	public string CSPAnalytics { get; set; }

	[DataMember]
	public string XFrameOptions { get; set; }

	[DataMember]
	public List<string> AllowOrigins { get; set; } = new List<string>();

	[DataMember]
	public string AllowMethods { get; set; }

	[DataMember]
	public string AllowHeaders { get; private set; }

	[DataMember]
	public string XXssProtection { get; private set; }

	[DataMember]
	public string StrictTransportSecurity { get; private set; }

	[DataMember]
	public bool ConnectionId { get; private set; }

	public string SectionName => "HttpHeaders";

	public HttpHeadersConfig(string contentSecurityPolicy, string xFrameOptions, string allowMethods, string allowHeaders, string xXssProtection, string strictTransportSecurity)
	{
		ContentSecurityPolicy = contentSecurityPolicy;
		XFrameOptions = xFrameOptions;
		AllowMethods = allowMethods;
		AllowHeaders = allowHeaders;
		XXssProtection = xXssProtection;
		StrictTransportSecurity = strictTransportSecurity;
		ConnectionId = false;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			ContentSecurityPolicy = section["Content-Security-Policy"];
			CSPAnalytics = section["CSP-Analytics"];
			XFrameOptions = section["X-Frame-Options"].Sanitized();
			if (section != null && section.AllKeys.Contains("Access-Control-Allow-Origin"))
			{
				AllowOrigins = (from e in section["Access-Control-Allow-Origin"].Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries)
					select e.Trim()).ToList();
			}
			if (section != null && section.AllKeys.Contains("Access-Control-Allow-Methods"))
			{
				AllowMethods = section["Access-Control-Allow-Methods"].Sanitized();
			}
			if (section != null && section.AllKeys.Contains("Access-Control-Allow-Headers"))
			{
				AllowHeaders = section["Access-Control-Allow-Headers"].Sanitized();
			}
			XXssProtection = section["X-XSS-Protection"].Sanitized();
			StrictTransportSecurity = section["Strict-Transport-Security"].Sanitized();
			if (section != null && section.AllKeys.Contains("ConnectionId"))
			{
				ConnectionId = string.Compare(section["ConnectionId"].Sanitized(), true.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0;
			}
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["Content-Security-Policy"] = ContentSecurityPolicy;
			serverSection["X-Frame-Options"] = XFrameOptions;
			if (serverSection.AllKeys.Contains("Access-Control-Allow-Origin"))
			{
				serverSection["Access-Control-Allow-Origin"] = string.Join(",", AllowOrigins);
			}
			if (serverSection.AllKeys.Contains("Access-Control-Allow-Methods"))
			{
				serverSection["Access-Control-Allow-Methods"] = AllowMethods;
			}
			if (serverSection.AllKeys.Contains("Access-Control-Allow-Headers"))
			{
				serverSection["Access-Control-Allow-Headers"] = AllowHeaders;
			}
			serverSection["X-XSS-Protection"] = XXssProtection;
			serverSection["Strict-Transport-Security"] = StrictTransportSecurity;
			if (serverSection.AllKeys.Contains("ConnectionId"))
			{
				serverSection["ConnectionId"] = ConnectionId.ToString();
			}
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}
}
