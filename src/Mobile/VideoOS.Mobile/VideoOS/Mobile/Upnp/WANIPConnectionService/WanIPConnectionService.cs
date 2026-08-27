using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class WanIPConnectionService
{
	public const string WanIPConnectionNamespace = "urn:schemas-upnp-org:service:WANIPConnection:1";

	public const string WanIPConnectionServiceName = "urn:schemas-upnp-org:service:WANIPConnection:1";

	private const string AddPortMappingCmdName = "AddPortMapping";

	private const string DeletePortMappingCmdName = "DeletePortMapping";

	private const string GetGenericPortMappingEntryCmdName = "GetGenericPortMappingEntry";

	private const string GetExternalIpAddressCmdName = "GetExternalIPAddress";

	private readonly ISoapClient _soapClient;

	private readonly WANIPSanitizer _sanitizer;

	public WanIPConnectionService(string serviceUrl, ISoapClientFactory soapClientFactory, WANIPSanitizer sanitizer = null)
	{
		_sanitizer = sanitizer;
		_soapClient = soapClientFactory.Create("urn:schemas-upnp-org:service:WANIPConnection:1", serviceUrl, new SoapMessageSerializer("urn:schemas-upnp-org:service:WANIPConnection:1"));
	}

	public List<PortMapping> GetAllGenericPortMappings()
	{
		List<PortMapping> list = new List<PortMapping>();
		for (int i = 0; i <= 255; i++)
		{
			PortMapping genericPortMapping = GetGenericPortMapping(i);
			if (genericPortMapping == null)
			{
				break;
			}
			if (_sanitizer == null || _sanitizer.TrySanitize(genericPortMapping))
			{
				list.Add(genericPortMapping);
			}
		}
		return list;
	}

	public PortMapping GetGenericPortMapping(int index)
	{
		PortMapping result = null;
		try
		{
			GetGenericPortMappingEntry request = new GetGenericPortMappingEntry
			{
				NewPortMappingIndex = index
			};
			result = _soapClient.SendCommand<GetGenericPortMappingEntry, GetGenericPortMappingEntryResponse>(request, "GetGenericPortMappingEntry");
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return result;
	}

	public bool AddPortMapping(AddPortMapping portMapping)
	{
		bool flag = _sanitizer == null || _sanitizer.TrySanitize(portMapping);
		if (flag)
		{
			try
			{
				flag = _soapClient.SendCommand(portMapping, "AddPortMapping");
			}
			catch (Exception ex)
			{
				flag = false;
				Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
			}
		}
		return flag;
	}

	public bool DeleteForwardingRule(DeletePortMapping portMapping)
	{
		bool flag = _sanitizer == null || _sanitizer.TrySanitize(portMapping);
		try
		{
			if (flag)
			{
				flag = _soapClient.SendCommand(portMapping, "DeletePortMapping");
			}
		}
		catch (Exception ex)
		{
			flag = false;
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return flag;
	}

	public bool GetExternalIP(out IPAddress ipAddress)
	{
		bool result = true;
		ipAddress = IPAddress.Any;
		try
		{
			GetExternalIpAddress request = new GetExternalIpAddress();
			GetExternalIPAddressResponse getExternalIPAddressResponse = _soapClient.SendCommand<GetExternalIpAddress, GetExternalIPAddressResponse>(request, "GetExternalIPAddress");
			ipAddress = IPAddress.Parse(getExternalIPAddressResponse.NewExternalIPAddress);
		}
		catch (Exception ex)
		{
			result = false;
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return result;
	}

	public static string GetServiceUrl(string deviceDesc, Uri uri)
	{
		string result = string.Empty;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (StringReader input = new StringReader(deviceDesc))
			{
				using XmlReader reader = XmlReader.Create(input, SecurityXmlReaderSettings.Settings);
				xmlDocument.Load(reader);
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			xmlNamespaceManager.AddNamespace("tns", "urn:schemas-upnp-org:device-1-0");
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//tns:device/tns:deviceType/text()", xmlNamespaceManager);
			if (xmlNode == null || !xmlNode.Value.Contains("InternetGatewayDevice"))
			{
				return null;
			}
			XmlNode xmlNode2 = xmlDocument.SelectSingleNode("//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:controlURL/text()", xmlNamespaceManager);
			if (xmlNode2 == null)
			{
				return null;
			}
			result = new UriBuilder(uri.Scheme, uri.Host, uri.Port, xmlNode2.Value.TrimStart("/\\".ToCharArray())).ToString();
		}
		catch (Exception ex)
		{
			Log.Error("WanIPConnectionService.GetServiceUrl", ex.Message);
		}
		return result;
	}
}
