namespace VideoOS.Common.EnvironmentServices.Web;

public interface IWebRequest
{
	bool UseDefaultCredentials { get; set; }

	int Timeout { get; set; }

	IWebResponse GetResponse();
}
