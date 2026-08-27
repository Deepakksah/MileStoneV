using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class DiscoveryResponse : DiscoveryResponseBase
{
	public int ServerMajorVersion { get; }

	public int ServerMinorVersion { get; }

	public string ServerVersion => $"{ServerMajorVersion}.{ServerMinorVersion}";

	public DiscoveryResponse(HttpStatusCode statusCode, string reason)
		: base(statusCode, reason)
	{
		ServerMajorVersion = -1;
		ServerMinorVersion = -1;
	}

	public DiscoveryResponse(Exception exception, string reason)
		: base(exception, reason)
	{
		ServerMajorVersion = -1;
		ServerMinorVersion = -1;
	}

	public DiscoveryResponse(Uri identityProviderUri, string httpContent)
		: base(identityProviderUri, httpContent)
	{
		try
		{
			dynamic val = JsonConvert.DeserializeObject(httpContent);
			string[] array = ((string)val.server_version).Split('.');
			ServerMajorVersion = Convert.ToInt32(array[0], CultureInfo.InvariantCulture);
			ServerMinorVersion = Convert.ToInt32((array[1] != null) ? array[1] : "0", CultureInfo.InvariantCulture);
		}
		catch (Exception)
		{
			ServerMajorVersion = -1;
			ServerMinorVersion = -1;
			throw;
		}
	}

	public DiscoveryResponse(string reason, DiscoveryResponse[] errorResponses)
		: base(reason, errorResponses)
	{
		ServerMajorVersion = -1;
		ServerMinorVersion = -1;
	}
}
