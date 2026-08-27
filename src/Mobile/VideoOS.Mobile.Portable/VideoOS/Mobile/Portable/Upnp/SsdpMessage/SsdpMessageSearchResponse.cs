using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Mobile.Portable.Http;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpMessageSearchResponse : HttpMessageResponse
{
	private static readonly List<string> _requiredParams = new List<string> { "CACHE-CONTROL", "LOCATION", "ST", "EXT", "SERVER", "USN" };

	public int CacheControl
	{
		get
		{
			return int.Parse(GetHeaderValue("CACHE-CONTROL").Split("=".ToCharArray())[1]);
		}
		set
		{
			AddHeader("CACHE-CONTROL", $"max-age={value}");
		}
	}

	public string Location
	{
		get
		{
			return GetHeaderValue("LOCATION");
		}
		set
		{
			AddHeader("LOCATION", value);
		}
	}

	public string SearchType
	{
		get
		{
			return GetHeaderValue("ST");
		}
		set
		{
			AddHeader("ST", value);
		}
	}

	public string Server
	{
		get
		{
			return GetHeaderValue("SERVER");
		}
		set
		{
			AddHeader("SERVER", value);
		}
	}

	public string Usn
	{
		get
		{
			return GetHeaderValue("USN");
		}
		set
		{
			AddHeader("USN", value);
		}
	}

	public SsdpMessageSearchResponse(HttpStatuses statusCode)
		: base(statusCode)
	{
		AddHeader("EXT", string.Empty);
		AddHeader("DATE", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.InvariantCulture));
	}

	private SsdpMessageSearchResponse(HttpMessageResponse response)
		: base(response)
	{
	}

	public SsdpMessageSearchResponse(int maxAge, string location, string searchType, string identifier, string server)
		: this(HttpStatuses.Ok)
	{
		CacheControl = maxAge;
		Location = location;
		SearchType = searchType;
		Usn = identifier;
		Server = server;
	}

	public static bool TryParse(string message, out SsdpMessageSearchResponse messageRequest)
	{
		bool result = false;
		messageRequest = null;
		try
		{
			if (HttpMessageResponse.TryParse(message, out var httpMessage, _requiredParams))
			{
				messageRequest = new SsdpMessageSearchResponse(httpMessage);
				result = true;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "SsdpMessageSearchResponse.TryParse", ex.Message);
		}
		return result;
	}
}
