using System;
using System.Net.Http;
using VideoOS.IM.Common.Service.Properties;

namespace VideoOS.IM.Common.Service.Builders.HttpRequestMessages;

public interface IHttpRequestMessageBuilder
{
	[NotNull]
	HttpRequestMessage Build();

	[NotNull]
	IHttpRequestMessageBuilder WithHttpUri([NotNull] Uri uri);

	[NotNull]
	IHttpRequestMessageBuilder WithHttpMethod([NotNull] HttpMethod httpMethod);

	[NotNull]
	IHttpRequestMessageBuilder WithBearerToken([NotNull] string accessToken);

	[NotNull]
	IHttpRequestMessageBuilder WithHttpContent<TRequest>([NotNull] TRequest request) where TRequest : class;
}
