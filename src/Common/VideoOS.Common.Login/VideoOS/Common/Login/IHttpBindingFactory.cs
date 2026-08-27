using System.ServiceModel;

namespace VideoOS.Common.Login;

public interface IHttpBindingFactory
{
	BasicHttpBinding CreateHttpBinding(string address);

	BasicHttpBinding CreateHttpBinding(string address, int millisecondsTimeout);

	WSHttpBinding CreateHttpBindingCredentials(string address);

	WSHttpBinding CreateHttpBindingCredentials(string address, int millisecondsTimeout);
}
