namespace VideoOS.IdentityServer.Common.Handlers;

public class IdentityServerTokenResponse : TokenResponse
{
	public bool IsError { get; set; }

	public bool IsHttpError { get; set; }

	public int HttpStatusCode { get; set; }

	public string Error { get; set; }

	public string ErrorDescription { get; set; }
}
