namespace VideoOS.Mobile.Portable.Http.HttpMessage;

public class HttpResponseSanitizer
{
	public bool Sanitize(HttpMessageResponse response)
	{
		return HttpMessageValidator.ValidateHeaders(response.Headers);
	}
}
