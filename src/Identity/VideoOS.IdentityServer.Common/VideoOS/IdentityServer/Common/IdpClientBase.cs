using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common;

public abstract class IdpClientBase : IDisposable
{
	private readonly IHttpClient _client;

	private bool disposedValue;

	public Uri BaseAddress { get; }

	private static MediaTypeFormatterCollection MediaTypeFormatters
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0011: Expected O, but got Unknown
			MediaTypeFormatterCollection val = new MediaTypeFormatterCollection();
			((Collection<MediaTypeFormatter>)val).Add((MediaTypeFormatter)(object)new ProblemJsonMediaTypeFormatter());
			return val;
		}
	}

	public IdpClientBase(Uri baseAddress, IHttpClientFactory httpClientFactory)
		: this(baseAddress, httpClientFactory.Create())
	{
	}

	protected IdpClientBase(Uri baseAddress, IHttpClient client)
	{
		if (baseAddress == null)
		{
			throw new ArgumentNullException("baseAddress");
		}
		if (client == null)
		{
			throw new ArgumentNullException("client");
		}
		_client = client;
		BaseAddress = baseAddress;
	}

	protected Task Send(HttpRequestMessage request)
	{
		return Send(request, CancellationToken.None);
	}

	protected Task Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Send<object>(request, readContent: false, cancellationToken);
	}

	protected Task<T> SendAndReadContent<T>(HttpRequestMessage request)
	{
		return SendAndReadContent<T>(request, CancellationToken.None);
	}

	protected Task<T> SendAndReadContent<T>(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return Send<T>(request, readContent: true, cancellationToken);
	}

	private async Task<T> Send<T>(HttpRequestMessage request, bool readContent, CancellationToken cancellationToken)
	{
		using IHttpResponseMessage response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (!response.IsSuccessStatusCode)
		{
			await ThrowIdpException(response).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (readContent)
		{
			try
			{
				return await response.Content.ReadAsAsync<T, MediaTypeFormatter>((IEnumerable<MediaTypeFormatter>)MediaTypeFormatters, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (UnsupportedMediaTypeException ex)
			{
				UnsupportedMediaTypeException ex2 = ex;
				throw new InternalServerErrorException(new ErrorResponse
				{
					Error = ((Exception)(object)ex2).Message
				});
			}
		}
		return default(T);
	}

	protected Uri BuildFullUri(FormattableString path)
	{
		string path2 = EncodeStringFormatParts(path);
		return BuildFullUri(path2);
	}

	protected Uri BuildFullUri(string path)
	{
		string text = BaseAddress.ToString();
		return new UriBuilder(string.Concat(str2: new Uri(path, UriKind.Relative).ToString().TrimStart('/'), str0: text.TrimEnd('/'), str1: "/")).Uri;
	}

	private static string EncodeStringFormatParts(FormattableString parts)
	{
		object[] array = new object[parts.ArgumentCount];
		for (int i = 0; i < parts.ArgumentCount; i++)
		{
			if (parts.GetArguments()[i] == null)
			{
				throw new ArgumentException("Cannot contain null values.", "parts");
			}
			array[i] = HttpUtility.UrlEncode(parts.GetArguments()[i].ToString());
		}
		return string.Format(CultureInfo.InvariantCulture, parts.Format, array);
	}

	private static async Task ThrowIdpException(IHttpResponseMessage response)
	{
		ErrorResponse errorResponse = null;
		try
		{
			errorResponse = await response.Content.ReadAsAsync<ErrorResponse, MediaTypeFormatter>((IEnumerable<MediaTypeFormatter>)MediaTypeFormatters).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (UnsupportedMediaTypeException)
		{
		}
		throw IdpExceptionFactory.CreateException((int)response.StatusCode, errorResponse);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && _client != null)
			{
				_client.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
