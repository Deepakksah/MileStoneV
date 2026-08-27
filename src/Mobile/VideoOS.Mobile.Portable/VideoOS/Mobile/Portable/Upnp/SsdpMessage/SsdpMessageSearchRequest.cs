using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpMessageSearchRequest : SsdpMessageRequest
{
	public static int MxDefault = 0;

	private static readonly List<string> _requiredParams = new List<string> { "HOST", "MAN", "ST", "MX" };

	public int Mx
	{
		get
		{
			if (!int.TryParse(GetHeaderValue("MX"), out var result))
			{
				return MxDefault;
			}
			return result;
		}
		set
		{
			AddHeader("MX", value.ToString());
		}
	}

	public string SearchTarget
	{
		get
		{
			return GetHeaderValue("ST").Trim(" \"".ToCharArray());
		}
		private set
		{
			AddHeader("ST", value);
		}
	}

	public string Man => GetHeaderValue("MAN");

	public static List<string> RequiredParams => _requiredParams;

	public static string MethodType => EnumsHelper.GetDescription(SsdpMessageMethod.MSearch);

	private SsdpMessageSearchRequest()
		: base(SsdpMessageMethod.MSearch)
	{
		AddHeader("MAN", "\"ssdp:discover\"");
	}

	public SsdpMessageSearchRequest(HttpMessageRequest request)
		: base(request)
	{
	}

	public SsdpMessageSearchRequest(int maxDelay, string searchType)
		: this()
	{
		Mx = maxDelay;
		SearchTarget = searchType;
	}

	public static bool TryParse(string message, out SsdpMessageSearchRequest messageRequest)
	{
		bool result = false;
		messageRequest = null;
		try
		{
			if (HttpMessageRequest.TryParse(message, out var httpMessage, MethodType, RequiredParams) && HttpMessageValidator.ValidateHeaders(httpMessage.Headers))
			{
				messageRequest = new SsdpMessageSearchRequest(httpMessage);
				result = true;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "SsdpMessageAnnounce.TryParse", ex.Message);
		}
		return result;
	}
}
