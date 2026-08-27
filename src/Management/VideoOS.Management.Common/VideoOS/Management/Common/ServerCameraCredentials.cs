using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Management.Common;

public class ServerCameraCredentials : List<ServerCameraCredential>
{
	public static string GetAsXml(ServerCameraCredentials cameraCredentials)
	{
		if (cameraCredentials == null)
		{
			throw new ArgumentNullException("cameraCredentials");
		}
		return GetAsXml(cameraCredentials.ToArray());
	}

	public static string GetAsXml(ServerCameraCredential[] cameraCredentials)
	{
		if (cameraCredentials == null)
		{
			throw new ArgumentNullException("cameraCredentials");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<CameraCredentials></CameraCredentials>");
		if (cameraCredentials != null)
		{
			foreach (ServerCameraCredential serverCameraCredential in cameraCredentials)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "CameraCredential", null);
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "userName", null);
				xmlNode2.Value = serverCameraCredential.UserName;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "password", null);
				xmlNode2.Value = serverCameraCredential.Password;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static ServerCameraCredentials GetFromXml(string cameraCredentialsXml)
	{
		ServerCameraCredentials serverCameraCredentials = new ServerCameraCredentials();
		if (!string.IsNullOrEmpty(cameraCredentialsXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(cameraCredentialsXml);
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (documentElement.Name == "CameraCredentials")
			{
				foreach (XmlNode childNode in documentElement.ChildNodes)
				{
					if (!(childNode.Name == "CameraCredential"))
					{
						continue;
					}
					string password = null;
					XmlNode namedItem = childNode.Attributes.GetNamedItem("userName");
					if (namedItem != null)
					{
						string value = namedItem.Value;
						namedItem = childNode.Attributes.GetNamedItem("password");
						if (namedItem != null)
						{
							password = namedItem.Value;
						}
						ServerCameraCredential item = new ServerCameraCredential(value, password);
						serverCameraCredentials.Add(item);
					}
				}
			}
		}
		return serverCameraCredentials;
	}
}
