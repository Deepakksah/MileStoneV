using System;

namespace VideoOS.Mobile.Portable.Net;

public interface IMulticastClient
{
	string LocalAddress { get; }

	short Ttl { get; set; }

	IPAddress MulticastAddress { get; }

	void BeginReceive(AsyncCallback requestCallback, object state);

	void Send(byte[] data, int length, int port);

	void Send(byte[] data, int length, string ipAddress, int port);

	byte[] EndReceive(IAsyncResult result, ref string remote);

	void Close();
}
