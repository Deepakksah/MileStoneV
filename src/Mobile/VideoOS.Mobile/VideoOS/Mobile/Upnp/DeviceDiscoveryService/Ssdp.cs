using System;
using System.Net;
using System.Text;
using System.Threading;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Net;
using VideoOS.Mobile.Portable.Upnp;
using VideoOS.Mobile.Portable.Upnp.SsdpMessage;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Utilities;

namespace VideoOS.Mobile.UPNP.DeviceDiscoveryService;

public class Ssdp : IDiscoveryProtocol
{
	private const short DeafultPacketTtl = 4;

	private readonly UpnpDeviceDescription _deviceDescription;

	private readonly string _uuid;

	private const int RepeatMessageCount = 2;

	private HttpRequestSanitizer _requestSanitizer = new HttpRequestSanitizer();

	private HttpResponseSanitizer _responseSanitizer = new HttpResponseSanitizer();

	public string XmlHttpPath { get; set; }

	public ushort XmlHttpPort { get; set; }

	private string Server => $"{SystemEnvironment.Instance.OsVersion} UpnP/1.0 {_deviceDescription.Device.ModelName}/{_deviceDescription.Device.ModelNumber}";

	public short PacketTtl => 4;

	public Ssdp(UpnpDeviceDescription deviceDescription)
	{
		System.Net.IPAddress.Parse("239.255.255.250");
		_uuid = deviceDescription.Device.Udn;
		_deviceDescription = deviceDescription;
	}

	public void ProcessRequest(IMulticastClient client, string request, IPEndPoint endPoint)
	{
		if (!SsdpMessageSearchRequest.TryParse(request.ToLower(), out var messageRequest) || messageRequest.SsdpMethod != SsdpMessageRequest.SsdpMessageMethod.MSearch)
		{
			return;
		}
		if (messageRequest.Mx > 5)
		{
			messageRequest.Mx = 5;
		}
		bool flag = messageRequest.Host.Equals(SsdpConstants.UpnpMulticastEndPoint) || messageRequest.Host.Equals(SsdpConstants.UpnpMulticastEndPointIPv6);
		if (!messageRequest.SearchTarget.Equals("ssdp:all", StringComparison.CurrentCultureIgnoreCase) && !messageRequest.SearchTarget.Equals("upnp:rootdevice", StringComparison.CurrentCultureIgnoreCase) && !messageRequest.SearchTarget.ToLower().Contains(_uuid.ToLower()) && !messageRequest.SearchTarget.ToLower().Contains(_deviceDescription.Device.DeviceType.ToLower()))
		{
			return;
		}
		if (flag)
		{
			if (messageRequest.Mx == SsdpMessageSearchRequest.MxDefault)
			{
				return;
			}
			Thread.Sleep(System.Math.Abs(new Random().Next(1, int.MaxValue) % (messageRequest.Mx * 1000)));
		}
		System.Net.IPAddress address = System.Net.IPAddress.Parse(client.LocalAddress);
		if (messageRequest.SearchTarget.Equals("ssdp:all", StringComparison.CurrentCultureIgnoreCase))
		{
			SendMessage(client, endPoint, GenerateXmlSearchResponse(address, "upnp:rootdevice"));
			SendMessage(client, endPoint, GenerateXmlSearchResponse(address, _uuid));
			SendMessage(client, endPoint, GenerateXmlSearchResponse(address, _deviceDescription.Device.DeviceType));
		}
		else
		{
			SendMessage(client, endPoint, GenerateXmlSearchResponse(address, messageRequest.SearchTarget));
		}
	}

	public void AnouncePacket(IMulticastClient udpClient, bool isByeBye)
	{
		IPEndPoint endPoint = null;
		if (udpClient.MulticastAddress.AddressFamily == AddressFamily.InterNetwork)
		{
			endPoint = new IPEndPoint(System.Net.IPAddress.Parse("239.255.255.250"), 1900);
		}
		else if (udpClient.MulticastAddress.AddressFamily == AddressFamily.InterNetworkV6)
		{
			endPoint = new IPEndPoint(System.Net.IPAddress.Parse("[FF02::C]"), 1900);
		}
		if (!isByeBye)
		{
			SendAnnounceMessage(udpClient, endPoint, "upnp:rootdevice");
			SendAnnounceMessage(udpClient, endPoint, _uuid);
			SendAnnounceMessage(udpClient, endPoint, _deviceDescription.Device.DeviceType);
		}
		else
		{
			SendMessage(udpClient, endPoint, new SsdpMessageByeBye("upnp:rootdevice", GetUsn("upnp:rootdevice")).GenerateMessage(_requestSanitizer));
			SendMessage(udpClient, endPoint, new SsdpMessageByeBye(_uuid, GetUsn(_uuid)).GenerateMessage(_requestSanitizer));
			SendMessage(udpClient, endPoint, new SsdpMessageByeBye(_deviceDescription.Device.DeviceType, GetUsn(_deviceDescription.Device.DeviceType)).GenerateMessage(_requestSanitizer));
		}
	}

	private void SendAnnounceMessage(IMulticastClient udpClient, IPEndPoint endPoint, string upnpRootDevice)
	{
		System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse(udpClient.LocalAddress);
		SendMessage(udpClient, endPoint, new SsdpMessageAnnounce(upnpRootDevice, 1800, GetXmlDescriptionLocation(ipAddress), Server, GetUsn(upnpRootDevice)).GenerateMessage(_requestSanitizer));
	}

	private void SendMessage(IMulticastClient client, IPEndPoint endPoint, string response)
	{
		if (!string.IsNullOrEmpty(response))
		{
			byte[] bytes = Encoding.UTF8.GetBytes(response);
			for (int i = 0; i < 2; i++)
			{
				client.Send(bytes, bytes.Length, endPoint.Address.ToString(), endPoint.Port);
				Thread.Sleep(10);
			}
		}
	}

	private string GenerateXmlSearchResponse(System.Net.IPAddress address, string notificationType)
	{
		return new SsdpMessageSearchResponse(1810, GetXmlDescriptionLocation(address), notificationType, GetUsn(notificationType), Server).GenerateMessage(_responseSanitizer);
	}

	private string GetUsn(string notificationType)
	{
		string result = string.Empty;
		if (notificationType.Equals("upnp:rootdevice", StringComparison.CurrentCultureIgnoreCase))
		{
			result = $"{_uuid}::upnp:rootdevice";
		}
		else if (notificationType.Equals(_uuid, StringComparison.CurrentCultureIgnoreCase))
		{
			result = _uuid;
		}
		else if (notificationType.Equals(_deviceDescription.Device.DeviceType, StringComparison.CurrentCultureIgnoreCase))
		{
			result = $"{_uuid}::{_deviceDescription.Device.DeviceType}";
		}
		return result;
	}

	private string GetXmlDescriptionLocation(System.Net.IPAddress ipAddress)
	{
		return new UriBuilder(Uri.UriSchemeHttp, ipAddress.GetIpAddress(), XmlHttpPort, XmlHttpPath).ToString();
	}
}
