namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public interface ISoapClientFactory
{
	ISoapClient Create(string serviceName, string serviceUrl, SoapMessageSerializer serializer);
}
