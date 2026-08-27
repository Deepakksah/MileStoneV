using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace VideoOS.Installer.Common.Utilities;

public static class ConfigXmlUtilities
{
	public static void UpdateConfigFileVersion(string configFilePath, string version)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		try
		{
			UpdateConfigFileVersion(xmlDocument, version);
		}
		finally
		{
			xmlDocument.Save(configFilePath);
		}
	}

	public static void UpdateConfigFileVersion(string configFilePath, ISession session)
	{
		if (session == null)
		{
			throw new ArgumentNullException("session");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		string version = session["PRODUCTVERSION"];
		try
		{
			UpdateConfigFileVersion(xmlDocument, version);
		}
		catch (ConfigXmlParseVersionFailedException ex)
		{
			session.Log(ex.Message);
		}
		xmlDocument.Save(configFilePath);
	}

	public static void UpdateConfigFileVersion(XmlDocument doc, string version)
	{
		Match match = Regex.Match(version, "^(?<Major>\\d+)\\.(?<Minor>\\d+)(?<Revision>[a-z])?$", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			throw new ConfigXmlParseVersionFailedException(string.Format(CultureInfo.InvariantCulture, "UpdateConfigFileVersion(): Product version could not be parsed corretly. Product version '{0}'", version));
		}
		string text = "0";
		string text2 = "0";
		if (match.Groups["Revision"].Success)
		{
			text2 = (match.Groups["Revision"].Value[0] - 97 + 1).ToString(CultureInfo.InvariantCulture);
		}
		string innerText = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", match.Groups["Major"].Value, match.Groups["Minor"].Value, text, text2);
		doc.DocumentElement.SelectSingleNode("version").InnerText = innerText;
	}

	public static string GetVersionOfConfigFile(string configFilePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		return GetVersionOfConfigFile(xmlDocument);
	}

	public static string GetVersionOfConfigFile(XmlDocument doc)
	{
		XmlNode xmlNode = doc.DocumentElement.SelectSingleNode("version");
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "0.0";
	}

	public static string GetClientTokenTimeout(string configFilePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		return GetClientTokenTimeout(xmlDocument);
	}

	public static string GetClientTokenTimeout(XmlDocument doc)
	{
		XmlNode xmlNode = doc.DocumentElement.SelectSingleNode("//server/tokentimeout/client");
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "240";
	}

	public static void SetAuthorizationServerAddress(string configFilePath, string nodePath, string authorizationServerAddress)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		SetAuthorizationServerAddress(xmlDocument, nodePath, authorizationServerAddress).Save(configFilePath);
	}

	public static XmlDocument SetAuthorizationServerAddress(XmlDocument doc, string nodePath, string authorizationServerAddress)
	{
		doc.SelectSingleNode(nodePath).InnerText = authorizationServerAddress;
		return doc;
	}

	public static void UpdateClientRegistrationId(string configFilePath, string nodePath, string id)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		UpdateClientRegistrationId(xmlDocument, nodePath, id).Save(configFilePath);
	}

	public static XmlDocument UpdateClientRegistrationId(XmlDocument doc, string nodePath, string id)
	{
		doc.SelectSingleNode(nodePath).InnerText = id;
		return doc;
	}

	public static string GetClientRegistrationId(string configFilePath, string nodePath)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(configFilePath);
		return GetClientRegistrationId(xmlDocument, nodePath);
	}

	public static string GetClientRegistrationId(XmlDocument doc, string nodePath)
	{
		XmlNode xmlNode = doc.DocumentElement.SelectSingleNode(nodePath);
		if (xmlNode != null && !string.IsNullOrEmpty(xmlNode.InnerText))
		{
			return xmlNode.InnerText;
		}
		return Guid.NewGuid().ToString();
	}
}
