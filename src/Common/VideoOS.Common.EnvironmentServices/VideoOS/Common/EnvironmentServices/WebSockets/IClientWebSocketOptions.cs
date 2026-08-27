namespace VideoOS.Common.EnvironmentServices.WebSockets;

public interface IClientWebSocketOptions
{
	void SetRequestHeader(string headerName, string headerValue);
}
