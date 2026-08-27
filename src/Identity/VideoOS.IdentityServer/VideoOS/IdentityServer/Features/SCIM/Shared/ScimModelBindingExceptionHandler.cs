using System;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public class ScimModelBindingExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		string text = httpContext.Features.Get<IExceptionHandlerPathFeature>()?.Path;
		if (text == null || !text.StartsWith("/scim", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (!(exception is BadHttpRequestException))
		{
			return false;
		}
		string text2 = ExtractParameterName(exception.Message);
		string detail = (text2.IsNullOrEmpty() ? "Invalid request parameter." : ("Invalid value provided for parameter: " + text2 + "."));
		var value = new
		{
			schemas = new string[1] { "urn:ietf:params:scim:api:messages:2.0:Error" },
			detail = detail,
			status = "400"
		};
		httpContext.Response.StatusCode = 400;
		await httpContext.Response.WriteAsJsonAsync(value, cancellationToken);
		return true;
	}

	[GeneratedRegex("Failed to bind parameter\\s+\"(.+?)\"\\s+from")]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "8.0.12.26602")]
	private static Regex ExtractParameterNameFromException()
	{
		return _003CRegexGenerator_g_003EF8B4B15C12CFF4DE373E8A565883FE4E442A37CEC738CDA9F7B3B3FB66249F0DF__ExtractParameterNameFromException_0.Instance;
	}

	private static string ExtractParameterName(string exceptionMessage)
	{
		if (string.IsNullOrEmpty(exceptionMessage))
		{
			return string.Empty;
		}
		Match match = ExtractParameterNameFromException().Match(exceptionMessage);
		if (!match.Success || match.Groups.Count < 2)
		{
			return string.Empty;
		}
		string[] array = match.Groups[1].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (array.Length <= 1)
		{
			return string.Empty;
		}
		return array[1];
	}
}
