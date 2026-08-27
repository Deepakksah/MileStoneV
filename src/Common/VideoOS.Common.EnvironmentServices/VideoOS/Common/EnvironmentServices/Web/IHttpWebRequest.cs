namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpWebRequest : IWebRequest
{
	bool HaveResponse { get; }
}
