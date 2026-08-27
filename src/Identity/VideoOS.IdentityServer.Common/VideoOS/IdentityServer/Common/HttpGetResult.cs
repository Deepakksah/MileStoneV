namespace VideoOS.IdentityServer.Common;

public sealed class HttpGetResult : IHttpGetResult
{
	public string Response { get; }

	public bool IsValidCertificate { get; }

	public HttpGetResult(string response, bool validCertificate)
	{
		Response = response;
		IsValidCertificate = validCertificate;
	}
}
