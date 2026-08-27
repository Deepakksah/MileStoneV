using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace VideoOS.IdentityServer.Common;

public class ProblemJsonMediaTypeFormatter : JsonMediaTypeFormatter
{
	public ProblemJsonMediaTypeFormatter()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		((MediaTypeFormatter)this).SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
	}
}
