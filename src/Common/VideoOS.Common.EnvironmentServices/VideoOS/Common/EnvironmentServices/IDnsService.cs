using System;
using System.Net;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices;

public interface IDnsService
{
	IAsyncResult BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback requestCallback, object state);

	IAsyncResult BeginGetHostEntry(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject);

	IAsyncResult BeginGetHostEntry(IPAddress address, AsyncCallback requestCallback, object stateObject);

	IPAddress[] EndGetHostAddresses(IAsyncResult asyncResult);

	IPHostEntry EndGetHostEntry(IAsyncResult asyncResult);

	IPAddress[] GetHostAddresses(string hostNameOrAddress);

	Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress);

	IPHostEntry GetHostEntry(IPAddress address);

	IPHostEntry GetHostEntry(string hostNameOrAddress);

	Task<IPHostEntry> GetHostEntryAsync(IPAddress address);

	Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress);

	string GetHostName();
}
