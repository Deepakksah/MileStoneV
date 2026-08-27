using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace VideoOS.Common.Integration.WebApi;

public class ContentStatusCodeResult : StatusCodeResult
{
	public ObjectContent Content { get; set; }

	public ContentStatusCodeResult(HttpStatusCode statusCode, HttpRequestMessage request)
		: this(statusCode, request, null)
	{
	}

	public ContentStatusCodeResult(HttpStatusCode statusCode, HttpRequestMessage request, object content)
		: base(statusCode, request)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (content != null)
		{
			Content = new ObjectContent(content.GetType(), content, (MediaTypeFormatter)new JsonMediaTypeFormatter());
		}
	}

	public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		HttpResponseMessage val = await _003C_003En__0(cancellationToken);
		if (Content != null)
		{
			val.Content = (HttpContent)(object)Content;
		}
		return val;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<HttpResponseMessage> _003C_003En__0(CancellationToken cancellationToken)
	{
		return ((StatusCodeResult)this).ExecuteAsync(cancellationToken);
	}
}
