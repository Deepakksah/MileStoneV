using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpMessageByeBye : SsdpMessageRequest
{
	private static readonly List<string> _requiredParams = new List<string> { "NT", "USN" };

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

	public static List<string> RequiredParams => _requiredParams;

	public static string MethodType => EnumsHelper.GetDescription(SsdpMessageMethod.Notify);

	public SsdpMessageByeBye()
		: base(SsdpMessageMethod.Notify)
	{
		Nts = "ssdp:byebye";
	}

	public SsdpMessageByeBye(string notificationType, string identifier)
		: this()
	{
		Nt = notificationType;
		Usn = identifier;
	}

	public SsdpMessageByeBye(HttpMessageRequest httpMessage)
		: base(httpMessage)
	{
	}

	public static bool TryParse(string message, out SsdpMessageByeBye messageRequest)
	{
		bool result = false;
		messageRequest = null;
		try
		{
			if (HttpMessageRequest.TryParse(message, out var httpMessage, MethodType, RequiredParams) && HttpMessageValidator.ValidateHeaders(httpMessage.Headers))
			{
				messageRequest = new SsdpMessageByeBye(httpMessage);
				result = true;
			}
		}
		catch (Exception)
		{
		}
		return result;
	}
}
