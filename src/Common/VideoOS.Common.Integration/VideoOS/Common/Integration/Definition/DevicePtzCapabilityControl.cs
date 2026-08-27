using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.Integration.Definition;

public class DevicePtzCapabilityControl
{
	public Guid CapabilityId = Guid.Empty;

	public string CapabilityName = string.Empty;

	public string DisplayName = string.Empty;

	public bool Absolute;

	public bool Relative;

	public bool Automatic;

	public bool Start;

	public bool Stop;

	public bool Speed;

	public DevicePtzCapabilityControl(Guid capabilityId, string capabilityName, string displayName, string xml)
	{
		CapabilityId = capabilityId;
		CapabilityName = capabilityName;
		DisplayName = displayName;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			try
			{
				XmlNode xmlNode = xmlDocument.DocumentElement["absolutesupport"];
				if (xmlNode != null)
				{
					Absolute = Convert.ToBoolean(xmlNode.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				XmlNode xmlNode2 = xmlDocument.DocumentElement["relativesupport"];
				if (xmlNode2 != null)
				{
					Relative = Convert.ToBoolean(xmlNode2.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				XmlNode xmlNode3 = xmlDocument.DocumentElement["stopsupport"];
				if (xmlNode3 != null)
				{
					Stop = Convert.ToBoolean(xmlNode3.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				XmlNode xmlNode4 = xmlDocument.DocumentElement["startsupport"];
				if (xmlNode4 != null)
				{
					Start = Convert.ToBoolean(xmlNode4.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				XmlNode xmlNode5 = xmlDocument.DocumentElement["automaticsupport"];
				if (xmlNode5 != null)
				{
					Automatic = Convert.ToBoolean(xmlNode5.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				XmlNode xmlNode6 = xmlDocument.DocumentElement["speedsupport"];
				if (xmlNode6 != null)
				{
					Speed = Convert.ToBoolean(xmlNode6.InnerText, CultureInfo.InvariantCulture);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}
}
