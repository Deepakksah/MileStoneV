namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public interface ISoapClient
{
	TResponse SendCommand<TRequest, TResponse>(TRequest request, string commandName);

	bool SendCommand<TRequest>(TRequest request, string commandName);
}
