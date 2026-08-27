using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VideoOS.Common.Integration.WebApi;

public static class WebApiExceptionHelper
{
	public static async Task<WebApiException> CreateResponseException(HttpResponseMessage response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		HttpRequestMessage requestMessage = response.RequestMessage;
		string route = ((requestMessage != null) ? requestMessage.RequestUri.ToString() : null);
		if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
		{
			ServiceUnavailableException ex = await ReadAndInitFromResponseMessage(new ServiceUnavailableException(), response);
			if (((HttpHeaders)response.Headers).Contains("Retry-After"))
			{
				ex.RetryAfterInSeconds = int.Parse(((HttpHeaders)response.Headers).GetValues("Retry-After").FirstOrDefault(), CultureInfo.InvariantCulture);
				return ex;
			}
			return ex;
		}
		if (response.StatusCode == HttpStatusCode.Forbidden)
		{
			return await ReadAndInitFromResponseMessage(new ForbiddenException(), response);
		}
		return CreateResponseException(route, response.StatusCode, null);
	}

	public static WebApiException CreateResponseException(string route, HttpStatusCode statusCode, Exception ex)
	{
		return statusCode switch
		{
			HttpStatusCode.InternalServerError => new InternalServerErrorException
			{
				StatusCode = statusCode,
				Route = route
			}, 
			HttpStatusCode.BadRequest => new BadRequestException
			{
				StatusCode = statusCode,
				Route = route
			}, 
			HttpStatusCode.Unauthorized => new UnauthorizedException
			{
				StatusCode = statusCode,
				Route = route
			}, 
			_ => new ResponseException(null, ex)
			{
				StatusCode = statusCode,
				Route = route
			}, 
		};
	}

	private static async Task<T> ReadAndInitFromResponseMessage<T>(T exception, HttpResponseMessage responseMessage) where T : ResponseException
	{
		if (responseMessage == null)
		{
			throw new ArgumentNullException("responseMessage");
		}
		JObject content = await HttpContentExtensions.ReadAsAsync<JObject>(responseMessage.Content);
		exception.StatusCode = responseMessage.StatusCode;
		exception.Content = content;
		HttpRequestMessage requestMessage = responseMessage.RequestMessage;
		exception.Route = ((requestMessage != null) ? requestMessage.RequestUri.ToString() : null);
		return exception;
	}
}
