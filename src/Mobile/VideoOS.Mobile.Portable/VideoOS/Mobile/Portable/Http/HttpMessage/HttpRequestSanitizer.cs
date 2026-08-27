namespace VideoOS.Mobile.Portable.Http.HttpMessage;

public class HttpRequestSanitizer
{
	public bool Sanitize(HttpMessageRequest request)
	{
		return HttpMessageValidator.ValidateHeaders(request.Headers);
	}
}
