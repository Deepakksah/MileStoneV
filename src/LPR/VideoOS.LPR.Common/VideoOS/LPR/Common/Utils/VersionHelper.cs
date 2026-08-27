using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Utils;

public static class VersionHelper
{
	public static Version GetEventServerVersion2(ServerId serverId)
	{
		try
		{
			List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray.Count > 0)
			{
				return GetEventServerVersion2(registeredServiceUriInfo[0]);
			}
		}
		catch (CommunicationMIPException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "LPR.VersionHelper", string.Format(CultureInfo.InvariantCulture, "Failed to get registered event server service {0}. Exception: {1}", serverId, ex.Message));
		}
		return null;
	}

	public static Version GetEventServerVersion2(Configuration.ServiceURIInfo eventServerUriInfo)
	{
		if (!string.IsNullOrEmpty(eventServerUriInfo.ServiceData))
		{
			XmlDocument xmlDocument = new XmlDocument
			{
				XmlResolver = null
			};
			using (XmlReader reader = XmlReader.Create(new StringReader(eventServerUriInfo.ServiceData), new XmlReaderSettings
			{
				XmlResolver = null
			}))
			{
				xmlDocument.Load(reader);
			}
			XmlNode xmlNode = xmlDocument.SelectSingleNode("EventServer/Version");
			if (xmlNode != null && xmlNode.HasChildNodes && !string.IsNullOrEmpty(xmlNode.InnerText))
			{
				Match match = Regex.Match(xmlNode.InnerText, "^(?<major>\\d+)\\.(?<minor>\\d+)(?<revision>[a-z]?)$");
				if (match.Success)
				{
					int minor = 0;
					int revision = 0;
					try
					{
						minor = int.Parse(match.Groups["minor"].Value, CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
					}
					try
					{
						revision = Math.Max(match.Groups["revision"].Value[0] - 97, 0);
					}
					catch (Exception)
					{
					}
					return new Version(int.Parse(match.Groups["major"].Value, CultureInfo.InvariantCulture), minor, 0, revision);
				}
			}
		}
		return null;
	}
}
