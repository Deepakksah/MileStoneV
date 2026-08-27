using System.Net;
using VideoOS.Mobile.Portable.Net;

namespace VideoOS.Mobile.UPNP.DeviceDiscoveryService;

public interface IDiscoveryProtocol
{
	short PacketTtl { get; }

	void AnouncePacket(IMulticastClient udpClient, bool isByeBye);

	void ProcessRequest(IMulticastClient client, string strData, IPEndPoint endPoint);
}
