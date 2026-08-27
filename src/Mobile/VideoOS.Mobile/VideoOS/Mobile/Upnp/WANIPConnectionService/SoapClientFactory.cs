namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class SoapClientFactory : ISoapClientFactory
{
	public ISoapClient Create(string serviceName, string serviceUrl, SoapMessageSerializer serializer)
	{
		return new SoapClient(serviceName, serviceUrl, serializer);
	}
}
