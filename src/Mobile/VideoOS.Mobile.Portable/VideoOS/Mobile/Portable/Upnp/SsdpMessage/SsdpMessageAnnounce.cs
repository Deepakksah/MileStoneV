using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpMessageAnnounce : SsdpMessageRequest
{
	private static readonly List<string> _requiredParams = new List<string> { "HOST", "CACHE-CONTROL", "LOCATION", "NT", "NTS", "SERVER", "USN" };

	public int CacheControl
	{
		get
		{
			return int.Parse(GetHeaderValue("CACHE-CONTROL").Split("=".ToCharArray())[1]);
		}
		private set
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
		private set
		{
			AddHeader("LOCATION", value);
		}
	}

	public string Nt
	{
		get
		{
			return GetHeaderValue("NT");
		}
		private set
		{
			AddHeader("NT", value);
		}
	}

	public string Nts
	{
		get
		{
			return GetHeaderValue("NTS");
		}
		private set
		{
			AddHeader("NTS", value);
		}
	}

	public string Server
	{
		get
		{
			return GetHeaderValue("SERVER");
		}
		private set
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
		private set
		{
			AddHeader("USN", value);
		}
	}

	public static List<string> RequiredParams => _requiredParams;

	public static string MethodType => EnumsHelper.GetDescription(SsdpMessageMethod.Notify);

	private SsdpMessageAnnounce()
		: base(SsdpMessageMethod.Notify)
	{
		Nts = "ssdp:alive";
	}

	public SsdpMessageAnnounce(HttpMessageRequest request)
		: base(request)
	{
	}

	public SsdpMessageAnnounce(string notificationType, int maxAge, string location, string server, string identifier)
		: this()
	{
		Nt = notificationType;
		CacheControl = maxAge;
		Location = location;
		Server = server;
		Usn = identifier;
	}

	public static bool TryParse(string message, out SsdpMessageAnnounce messageRequest)
	{
		bool result = false;
		messageRequest = null;
		try
		{
			if (HttpMessageRequest.TryParse(message, out var httpMessage, MethodType, RequiredParams))
			{
				messageRequest = new SsdpMessageAnnounce(httpMessage);
				result = true;
			}
		}
		catch (Exception)
		{
		}
		return result;
	}
}
