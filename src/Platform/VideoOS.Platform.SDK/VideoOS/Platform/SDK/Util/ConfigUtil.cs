using System;
using System.Xml;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Platform;

namespace VideoOS.Platform.SDK.Util;

public static class ConfigUtil
{
	public static string GenerateCameraConfigurationXml(FQID itemFQID)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)itemFQID.ServerId.UserContext.Configuration).FindSystemInfo(itemFQID.ServerId);
			if (systemInfo == null)
			{
				throw new MIPException("Server not found in configuration");
			}
			if (!systemInfo.Cameras.ContainsKey(itemFQID.ObjectId))
			{
				throw new MIPException("Camera not found in configuration");
			}
			ICamera camera = systemInfo.Cameras[itemFQID.ObjectId];
			IRecorder recorder = camera.Recorder;
			XmlDocument xmlDocument = camera.ConfigurationXml(Guid.Empty);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("camera");
			XmlNode xmlNode2 = xmlDocument.CreateElement("serverguid");
			xmlNode2.InnerText = systemInfo.Id.ToString();
			XmlNode xmlNode3 = xmlDocument.CreateElement("recorderguid");
			xmlNode3.InnerText = recorder.Guid.ToString();
			xmlNode.AppendChild(xmlNode2);
			xmlNode.AppendChild(xmlNode3);
			XmlNode xmlNode4 = xmlDocument.CreateElement("shortcut");
			xmlNode4.InnerText = "";
			xmlNode.AppendChild(xmlNode4);
			return xmlDocument.OuterXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("ConfigUtil.GenerateCameraConfigurationXml", exception);
			throw;
		}
	}

	public static string GenerateMicrophoneConfigurationXml(FQID itemFQID)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)itemFQID.ServerId.UserContext.Configuration).FindSystemInfo(itemFQID.ServerId);
			if (systemInfo == null)
			{
				throw new MIPException("Server not found in configuration");
			}
			if (!systemInfo.Microphones.ContainsKey(itemFQID.ObjectId))
			{
				throw new MIPException("Microphone not found in configuration");
			}
			IMicrophone microphone = systemInfo.Microphones[itemFQID.ObjectId];
			IRecorder recorder = microphone.Recorder;
			XmlDocument xmlDocument = microphone.ConfigurationXml();
			XmlNode xmlNode = xmlDocument.SelectSingleNode("audiodevice");
			XmlNode xmlNode2 = xmlDocument.CreateElement("serverguid");
			xmlNode2.InnerText = systemInfo.Id.ToString();
			XmlNode xmlNode3 = xmlDocument.CreateElement("recorderguid");
			xmlNode3.InnerText = recorder.Guid.ToString();
			xmlNode.AppendChild(xmlNode2);
			xmlNode.AppendChild(xmlNode3);
			XmlNode xmlNode4 = xmlDocument.CreateElement("shortcut");
			xmlNode4.InnerText = "";
			xmlNode.AppendChild(xmlNode4);
			return xmlDocument.OuterXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("ConfigUtil.GenerateMicrophoneConfigurationXml", exception);
			throw;
		}
	}

	public static string GenerateSpeakerConfigurationXml(FQID itemFQID)
	{
		try
		{
			ISystemInfo systemInfo = ((SDKConfiguration)itemFQID.ServerId.UserContext.Configuration).FindSystemInfo(itemFQID.ServerId);
			if (systemInfo == null)
			{
				throw new MIPException("Server not found in configuration");
			}
			if (!systemInfo.Speakers.ContainsKey(itemFQID.ObjectId))
			{
				throw new MIPException("Speaker not found in configuration");
			}
			Speaker speaker = systemInfo.Speakers[itemFQID.ObjectId];
			IRecorder recorder = speaker.Recorder;
			XmlDocument xmlDocument = speaker.ConfigurationXml();
			XmlNode xmlNode = xmlDocument.SelectSingleNode("audiodevice");
			XmlNode xmlNode2 = xmlDocument.CreateElement("serverguid");
			xmlNode2.InnerText = systemInfo.Id.ToString();
			XmlNode xmlNode3 = xmlDocument.CreateElement("recorderguid");
			xmlNode3.InnerText = recorder.Guid.ToString();
			xmlNode.AppendChild(xmlNode2);
			xmlNode.AppendChild(xmlNode3);
			XmlNode xmlNode4 = xmlDocument.CreateElement("shortcut");
			xmlNode4.InnerText = "";
			xmlNode.AppendChild(xmlNode4);
			return xmlDocument.OuterXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("ConfigUtil.GenerateSpeakerConfigurationXml", exception);
			throw;
		}
	}

	public static string StripXmlHeader(string xml)
	{
		if (string.IsNullOrEmpty(xml?.Trim()))
		{
			return xml;
		}
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
			{
				xmlDocument.RemoveChild(xmlDocument.FirstChild);
			}
			return xmlDocument.InnerXml;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("StripXmlHeader", exception);
			return xml;
		}
	}
}
