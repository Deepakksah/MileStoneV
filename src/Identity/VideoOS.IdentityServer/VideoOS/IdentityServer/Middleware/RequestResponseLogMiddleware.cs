using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using VideoOS.IdentityServer.Configuration;
using VideoOS.IdentityServer.Extensions;

namespace VideoOS.IdentityServer.Middleware;

public class RequestResponseLogMiddleware
{
	private readonly RequestDelegate _next;

	private readonly IConfiguration _configuration;

	private readonly ILogger<RequestResponseLogMiddleware> _logger;

	public RequestResponseLogMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<RequestResponseLogMiddleware> logger)
	{
		_next = next;
		_configuration = configuration;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!_configuration.Get<IdentityServerConfiguration>().LogRequestResponse)
		{
			await _next(context).IgnoreContext();
		}
		else
		{
			await FormatAndWriteRequestResponse(context).IgnoreContext();
		}
	}

	private async Task FormatAndWriteRequestResponse(HttpContext context)
	{
		using StringWriter writer = new StringWriter();
		context.Request.EnableBuffering();
		string value;
		if (context.Request.ContentLength < 1048576)
		{
			byte[] buffer = new byte[Convert.ToInt32(context.Request.ContentLength, CultureInfo.InvariantCulture)];
			await context.Request.Body.ReadAsync(buffer, 0, buffer.Length).IgnoreContext();
			value = Encoding.UTF8.GetString(buffer);
			context.Request.Body.Position = 0L;
		}
		else
		{
			value = "[Request body too big]";
		}
		writer.WriteLine();
		writer.WriteLine(context.Request.Method + " " + context.Request.GetDisplayUrl());
		FormatHeadersAndWrite(writer, context.Request.Headers);
		writer.WriteLine(value);
		using (MemoryStream responseBody = new MemoryStream())
		{
			Stream originalBodyStream = context.Response.Body;
			context.Response.Body = responseBody;
			Stopwatch watch = Stopwatch.StartNew();
			await _next(context).IgnoreContext();
			watch.Stop();
			context.Response.Body.Seek(0L, SeekOrigin.Begin);
			string value2 = await new StreamReader(context.Response.Body).ReadToEndAsync().IgnoreContext();
			context.Response.Body.Seek(0L, SeekOrigin.Begin);
			writer.WriteLine($"Response: {context.Response.StatusCode}");
			FormatHeadersAndWrite(writer, context.Response.Headers);
			writer.WriteLine(value2);
			writer.WriteLine($"Request finished in {watch.ElapsedMilliseconds} ms");
			await responseBody.CopyToAsync(originalBodyStream).IgnoreContext();
		}
		string message = writer.ToString();
		_logger.LogWarning(message);
	}

	private static void FormatHeadersAndWrite(StringWriter writer, IHeaderDictionary headers)
	{
		foreach (KeyValuePair<string, StringValues> header in headers)
		{
			string text = (header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase) ? "[Redacted]" : header.Value.ToString());
			writer.WriteLine(header.Key + ": " + text);
		}
	}
}
