using System.Collections.Generic;
using VideoOS.Mobile.Portable.Net;

namespace VideoOS.Mobile.Portable.Upnp;

public interface IMulticastClientProvider
{
	IEnumerable<IMulticastClient> GetClients();

	void RemoveNetworkInterface(IMulticastClient multicastClient);

	void ClearNetworkInterfaces();
}
