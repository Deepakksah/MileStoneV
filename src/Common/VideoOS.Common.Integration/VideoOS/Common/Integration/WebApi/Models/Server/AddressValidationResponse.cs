using System;
using System.Net;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class AddressValidationResponse
{
	public WebExceptionStatus WebExceptionStatus { get; set; }

	public string Message { get; set; }

	public AddressValidationResponse()
	{
	}

	public AddressValidationResponse(WebException webException)
	{
		if (webException == null)
		{
			throw new ArgumentNullException("webException");
		}
		WebExceptionStatus = webException.Status;
		Message = webException.Message;
	}
}
