using System;
using System.Net;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices;

public sealed class DnsService : IDnsService
{
	IAsyncResult IDnsService.BeginGetHostAddresses(string hostNameOrAddress, AsyncCallback requestCallback, object state)
	{
		return Dns.BeginGetHostAddresses(hostNameOrAddress, requestCallback, state);
	}

	IAsyncResult IDnsService.BeginGetHostEntry(string hostNameOrAddress, AsyncCallback requestCallback, object stateObject)
	{
		return Dns.BeginGetHostEntry(hostNameOrAddress, requestCallback, stateObject);
	}

	IAsyncResult IDnsService.BeginGetHostEntry(IPAddress address, AsyncCallback requestCallback, object stateObject)
	{
		return Dns.BeginGetHostEntry(address, requestCallback, stateObject);
	}

	IPAddress[] IDnsService.EndGetHostAddresses(IAsyncResult asyncResult)
	{
		return Dns.EndGetHostAddresses(asyncResult);
	}

	IPHostEntry IDnsService.EndGetHostEntry(IAsyncResult asyncResult)
	{
		return Dns.EndGetHostEntry(asyncResult);
	}

	IPAddress[] IDnsService.GetHostAddresses(string hostNameOrAddress)
	{
		return Dns.GetHostAddresses(hostNameOrAddress);
	}

	Task<IPAddress[]> IDnsService.GetHostAddressesAsync(string hostNameOrAddress)
	{
		return Dns.GetHostAddressesAsync(hostNameOrAddress);
	}

	IPHostEntry IDnsService.GetHostEntry(IPAddress address)
	{
		return Dns.GetHostEntry(address);
	}

	IPHostEntry IDnsService.GetHostEntry(string hostNameOrAddress)
	{
		return Dns.GetHostEntry(hostNameOrAddress);
	}

	Task<IPHostEntry> IDnsService.GetHostEntryAsync(IPAddress address)
	{
		return Dns.GetHostEntryAsync(address);
	}

	Task<IPHostEntry> IDnsService.GetHostEntryAsync(string hostNameOrAddress)
	{
		return Dns.GetHostEntryAsync(hostNameOrAddress);
	}

	string IDnsService.GetHostName()
	{
		return Dns.GetHostName();
	}
}
