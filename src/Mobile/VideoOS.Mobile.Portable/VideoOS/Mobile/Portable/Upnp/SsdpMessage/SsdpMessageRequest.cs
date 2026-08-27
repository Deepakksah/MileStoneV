using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpMessageRequest : HttpMessageRequest
{
	public enum SsdpMessageMethod
	{
		[Description("Unknown")]
		Unknown,
		[Description("NOTIFY")]
		Notify,
		[Description("M-SEARCH")]
		MSearch
	}

	public SsdpMessageMethod SsdpMethod
	{
		get
		{
			return EnumsHelper.EnumConverter<SsdpMessageMethod>(base.Method);
		}
		set
		{
			base.Method = EnumsHelper.GetDescription(value);
		}
	}

	public string Host => GetHeaderValue("HOST");

	public SsdpMessageRequest()
	{
		base.HttpVersion = "HTTP/1.1";
		base.RequestUri = "*";
		AddHeader("HOST", SsdpConstants.UpnpMulticastEndPoint);
	}

	public SsdpMessageRequest(SsdpMessageMethod method)
		: this()
	{
		SsdpMethod = method;
	}

	protected SsdpMessageRequest(HttpMessageRequest request)
		: base(request)
	{
	}
}
