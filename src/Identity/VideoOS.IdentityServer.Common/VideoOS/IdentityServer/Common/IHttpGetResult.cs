namespace VideoOS.IdentityServer.Common;

public interface IHttpGetResult
{
	string Response { get; }

	bool IsValidCertificate { get; }
}
