using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ClientVersionConfig
{
	public static ClientVersion GetSmartClientVersion(string clientVersionXml, IOemResources oemResources)
	{
		if (oemResources == null)
		{
			throw new ArgumentNullException("oemResources");
		}
		SmartClientUpgrade smartClientUpgrade = new SmartClientUpgrade();
		smartClientUpgrade.VersionList = new List<ForceUpgrade>();
		SmartClientVersion smartClientVersion = new SmartClientVersion();
		smartClientVersion.UpgradeInfo = new UpgradeInfo();
		ClientVersion result = new ClientVersion(smartClientVersion, smartClientUpgrade);
		ReadClientVersionXml(clientVersionXml, smartClientVersion, smartClientUpgrade, oemResources);
		return result;
	}

	private static void ReadClientVersionXml(string clientVersionXml, SmartClientVersion smartClientVersion, SmartClientUpgrade smartClientUpgrade, IOemResources oemResources)
	{
		XmlReaderSettings settings = new XmlReaderSettings
		{
			ConformanceLevel = ConformanceLevel.Auto,
			IgnoreComments = true,
			IgnoreWhitespace = true
		};
		using StringReader input = new StringReader(clientVersionXml);
		using XmlReader xmlReader = XmlReader.Create(input, settings);
		xmlReader.Read();
		if (xmlReader.NodeType == XmlNodeType.XmlDeclaration)
		{
			xmlReader.Read();
		}
		while (xmlReader.NodeType == XmlNodeType.Element)
		{
			string name = xmlReader.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (StringComparer.OrdinalIgnoreCase.Equals("smartclient", name))
				{
					ReadSmartClientElement(xmlReader, smartClientVersion, oemResources);
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("upgrade", name))
				{
					ReadUpgrade(xmlReader, smartClientUpgrade);
					continue;
				}
			}
			xmlReader.Read();
		}
		xmlReader.ReadEndElement();
	}

	private static void ReadSmartClientElement(XmlReader reader, SmartClientVersion smartClientVersion, IOemResources oemResources)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			string name = reader.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (StringComparer.OrdinalIgnoreCase.Equals("version", name))
				{
					ReadVersionsAttributes(reader, smartClientVersion);
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("upgradeinfo", name))
				{
					ReadUpgradeInfo(reader, smartClientVersion, oemResources);
					continue;
				}
			}
			reader.Read();
		}
		reader.ReadEndElement();
	}

	private static void ReadVersionsAttributes(XmlReader reader, SmartClientVersion smartClientVersion)
	{
		Version version = GeminiApplicationCore.ServerVersion();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name == null)
				{
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("major", name))
				{
					if (int.TryParse(reader.Value, out var result))
					{
						smartClientVersion.Major = result;
					}
					else
					{
						smartClientVersion.Major = version.Major;
					}
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("minor", name))
				{
					if (int.TryParse(reader.Value, out var result2))
					{
						smartClientVersion.Minor = result2;
					}
					else if (version.Major >= 12)
					{
						smartClientVersion.Minor = version.Minor;
					}
					else
					{
						smartClientVersion.Minor = version.Minor - 1;
					}
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("revision", name))
				{
					if (!StringComparer.OrdinalIgnoreCase.Equals("[SubstRevisionLetterVersion]", reader.Value))
					{
						smartClientVersion.Revision = reader.Value;
					}
					else
					{
						smartClientVersion.Revision = Convert.ToChar(version.Revision + 96).ToString();
					}
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("beta", name) && int.TryParse(reader.Value, out var result3))
				{
					smartClientVersion.Beta = result3;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("forceupgrade", name) && bool.TryParse(reader.Value, out var result4))
				{
					smartClientVersion.ForceUpgrade = result4;
					if (version.Major == 1 && version.Minor == 0)
					{
						smartClientVersion.ForceUpgrade = false;
					}
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("displayname", name))
				{
					if (!StringComparer.OrdinalIgnoreCase.Equals("[SubstVersion]", reader.Value))
					{
						smartClientVersion.DisplayName = reader.Value;
					}
					else if (version.Major >= 12)
					{
						smartClientVersion.DisplayName = FormattableString.Invariant($"{version.Major}.{version.Minor}{Convert.ToChar(version.Revision + 96)}");
					}
					else
					{
						smartClientVersion.DisplayName = FormattableString.Invariant($"{version.Major}.{version.Minor - 1}{Convert.ToChar(version.Revision + 96)}");
					}
				}
			}
		}
		reader.Read();
	}

	private static void ReadUpgradeInfo(XmlReader reader, SmartClientVersion smartClientVersion, IOemResources oemResources)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			string name = reader.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (StringComparer.OrdinalIgnoreCase.Equals("url", name))
				{
					ReadUrlElement(reader, smartClientVersion, oemResources);
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("message", name))
				{
					smartClientVersion.UpgradeInfo.Message = StringFromXml(reader);
					continue;
				}
			}
			reader.Read();
		}
		reader.ReadEndElement();
	}

	private static void ReadUrlElement(XmlReader reader, SmartClientVersion smartClientVersion, IOemResources oemResources)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			string name = reader.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (StringComparer.OrdinalIgnoreCase.Equals("hostname", name))
				{
					string text = StringFromXml(reader);
					if (StringComparer.OrdinalIgnoreCase.Equals("[SubstUrlHostName]", text) && Uri.TryCreate(oemResources.App.AddRemoveProgramUpdateUrl, UriKind.Absolute, out var result))
					{
						text = result.Host;
					}
					smartClientVersion.UpgradeInfo.Hostname = text;
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("port", name))
				{
					string text2 = StringFromXml(reader);
					if (StringComparer.OrdinalIgnoreCase.Equals("[SubstUrlPort]", text2) && Uri.TryCreate(oemResources.App.AddRemoveProgramUpdateUrl, UriKind.Absolute, out var result2))
					{
						text2 = FormattableString.Invariant($"{result2.Port}");
					}
					if (int.TryParse(text2, out var result3))
					{
						smartClientVersion.UpgradeInfo.Port = result3;
					}
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("path", name))
				{
					string text3 = StringFromXml(reader);
					if (StringComparer.OrdinalIgnoreCase.Equals("[SubstUrlPath]", text3) && Uri.TryCreate(oemResources.App.AddRemoveProgramUpdateUrl, UriKind.Absolute, out var result4))
					{
						text3 = result4.LocalPath;
					}
					smartClientVersion.UpgradeInfo.Path = text3;
					continue;
				}
			}
			reader.Read();
		}
		reader.ReadEndElement();
	}

	private static void ReadUpgrade(XmlReader reader, SmartClientUpgrade smartClientUpgrade)
	{
		while (reader.NodeType == XmlNodeType.Element)
		{
			string name = reader.Name;
			if (!string.IsNullOrEmpty(name))
			{
				if (StringComparer.OrdinalIgnoreCase.Equals("forcebeloworat", name))
				{
					ReadVersionAttributes(name, reader, smartClientUpgrade);
					continue;
				}
				if (StringComparer.OrdinalIgnoreCase.Equals("forcespecific", name))
				{
					ReadVersionAttributes(name, reader, smartClientUpgrade);
					continue;
				}
			}
			reader.Read();
		}
		reader.ReadEndElement();
	}

	private static void ReadVersionAttributes(string elementName, XmlReader reader, SmartClientUpgrade smartClientUpgrade)
	{
		ForceUpgrade forceUpgrade = new ForceUpgrade(elementName);
		forceUpgrade.Version = new SCVersion();
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null)
				{
					if (StringComparer.OrdinalIgnoreCase.Equals("major", name) && int.TryParse(reader.Value, out var result))
					{
						forceUpgrade.Version.Major = result;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals("minor", name) && int.TryParse(reader.Value, out var result2))
					{
						forceUpgrade.Version.Minor = result2;
					}
					if (StringComparer.OrdinalIgnoreCase.Equals("revision", name))
					{
						forceUpgrade.Version.Revision = reader.Value;
					}
				}
			}
		}
		smartClientUpgrade.VersionList.Add(forceUpgrade);
		reader.Read();
	}

	private static string StringFromXml(XmlReader reader)
	{
		string result = string.Empty;
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Text)
			{
				result = reader.Value;
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return result;
	}
}
