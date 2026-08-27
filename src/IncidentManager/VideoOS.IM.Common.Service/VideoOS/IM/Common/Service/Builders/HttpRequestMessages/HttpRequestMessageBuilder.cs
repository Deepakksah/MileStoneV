using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using VideoOS.IM.Common.Service.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Common.Service.Builders.HttpRequestMessages;

public sealed class HttpRequestMessageBuilder : IHttpRequestMessageBuilder
{
	[NotNull]
	private readonly HttpRequestMessage _httpRequestMessage = new HttpRequestMessage();

	public HttpRequestMessage Build()
	{
		return _httpRequestMessage;
	}

	public IHttpRequestMessageBuilder WithHttpUri([NotNull] Uri uri)
	{
		uri.NullCheck("", "WithHttpUri", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Common\\prod\\Service\\Builders\\HttpRequestMessages\\HttpRequestMessageBuilder.cs", 25);
		_httpRequestMessage.RequestUri = uri;
		return this;
	}

	public IHttpRequestMessageBuilder WithHttpMethod([NotNull] HttpMethod httpMethod)
	{
		httpMethod.NullCheck<HttpMethod>("", "WithHttpMethod", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Common\\prod\\Service\\Builders\\HttpRequestMessages\\HttpRequestMessageBuilder.cs", 35);
		_httpRequestMessage.Method = httpMethod;
		return this;
	}

	public IHttpRequestMessageBuilder WithBearerToken([NotNull] string accessToken)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		accessToken.NullCheck("", "WithBearerToken", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Common\\prod\\Service\\Builders\\HttpRequestMessages\\HttpRequestMessageBuilder.cs", 45);
		_httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		return this;
	}

	public IHttpRequestMessageBuilder WithHttpContent<TRequest>([NotNull] TRequest request) where TRequest : class
	{
		request.NullCheck("", "WithHttpContent", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Common\\prod\\Service\\Builders\\HttpRequestMessages\\HttpRequestMessageBuilder.cs", 55);
		_httpRequestMessage.Content = (HttpContent)(object)SerializeObject(request);
		return this;
	}

	private static StringContent SerializeObject<TObject>([NotNull] TObject inObject) where TObject : class
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		inObject.NullCheck("", "SerializeObject", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Common\\prod\\Service\\Builders\\HttpRequestMessages\\HttpRequestMessageBuilder.cs", 67);
		return new StringContent(JsonConvert.SerializeObject(inObject), Encoding.UTF8, "application/json");
	}
}
