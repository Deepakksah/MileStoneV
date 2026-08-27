namespace VideoOS.Toolkit;

public interface IServiceToolkit : IToolkit
{
	void ProcessRequest(string request, out string response);
}
