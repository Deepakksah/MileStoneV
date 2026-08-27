using System;
using System.Globalization;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public static class ExceptionHandling
{
	private static string MissingParametersTextId = "99999";

	public static ErrorModel ConvertToErrorModel(Exception ex)
	{
		if (ex == null)
		{
			throw new ArgumentNullException("ex");
		}
		if (ex.Message != "parameters missing" && !(ex is PathNotFoundException))
		{
			GeminiTrace.Error("RestAPI Exception", ex);
		}
		ErrorModel errorModel = new ErrorModel
		{
			HttpCode = HttpStatusCode.InternalServerError
		};
		PropertyException ex2 = ex as PropertyException;
		PropertyInvalidException ex3 = ex as PropertyInvalidException;
		if (ex is ArgumentNullException)
		{
			errorModel.AddText(string.Format(CultureInfo.InvariantCulture, "Bad request: {0} ", ex.Message));
			errorModel.HttpCode = HttpStatusCode.BadRequest;
		}
		else if (ex is ArgumentException)
		{
			string text = (ex.Source.Equals("System.IdentityModel.Tokens.Jwt", StringComparison.OrdinalIgnoreCase) ? "Unable to decode the header" : string.Format(CultureInfo.InvariantCulture, "Bad request: {0} ", ex.Message));
			if (ex.Message == "parameters missing")
			{
				errorModel.AddTextAndTextId(text, MissingParametersTextId);
			}
			else
			{
				errorModel.AddText(text);
			}
			errorModel.HttpCode = HttpStatusCode.BadRequest;
		}
		else if (ex is LicenseException)
		{
			errorModel.AddText("No license: " + ex.Message);
			errorModel.HttpCode = HttpStatusCode.Forbidden;
		}
		else if (ex is NotSupportedException)
		{
			string text2 = (string.IsNullOrEmpty(ex.Message) ? "Not supported" : ex.Message);
			errorModel.AddText(text2);
			errorModel.HttpCode = HttpStatusCode.BadRequest;
		}
		else if (ex is PathNotFoundException)
		{
			string text3 = (string.IsNullOrEmpty(ex.Message) ? "Path not found" : ex.Message);
			errorModel.AddText(text3);
			errorModel.HttpCode = HttpStatusCode.NotFound;
		}
		else if (ex is UnauthorizedException || ex is UnauthorizedAccessException || ex is TimeBasedUnauthorizedAccessException)
		{
			string text4 = (string.IsNullOrEmpty(ex.Message) ? "Access forbidden" : ex.Message);
			errorModel.AddText(text4);
			errorModel.HttpCode = HttpStatusCode.Forbidden;
		}
		else if (ex is ResourceNotFoundException)
		{
			errorModel.HttpCode = HttpStatusCode.NotFound;
			errorModel.AddText(ex.Message);
		}
		else if (ex is ServerException)
		{
			errorModel.AddText(ex.Message ?? string.Empty);
			errorModel.HttpCode = HttpStatusCode.NotFound;
		}
		else if (ex is TokenException)
		{
			errorModel.AddText("JSON syntax error");
			errorModel.HttpCode = HttpStatusCode.BadRequest;
		}
		else if (ex2 != null)
		{
			errorModel.HttpCode = HttpStatusCode.NotFound;
			errorModel.AddKeyText(ex2.PropertyKey, ex.Message);
		}
		else if (ex3 != null)
		{
			errorModel.HttpCode = HttpStatusCode.BadRequest;
			errorModel.AddKeyText(ex3.PropertyName, ex.Message);
		}
		else if (ex is SecurityTokenValidationException)
		{
			errorModel.HttpCode = HttpStatusCode.Unauthorized;
			string text5 = ((ex is SecurityTokenExpiredException) ? "Token validation failed: the token is expired" : "Token validation failed");
			errorModel.AddText(text5);
		}
		else
		{
			errorModel.HttpCode = HttpStatusCode.BadRequest;
			errorModel.AddText(ex.Message);
		}
		return errorModel;
	}
}
