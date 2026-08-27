using System;
using System.Linq;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DiffgramEncryption : IXmlTransformation
{
	public void Transform(XDocument doc, Func<string, byte[], string> method)
	{
		UpdateSystemSettings(doc, method);
		UpdateHardware(doc, method);
		UpdateRecordingStorage(doc, method);
		UpdateHostedHardware(doc, method);
		UpdateHostingServer(doc, method);
		UpdateLicense(doc, method);
	}

	private static void UpdateHostingServer(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace xNamespace = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(xNamespace + "HostingServer")
			select (d))
		{
			item.Element(xNamespace + "password").Value = method(item.Element(xNamespace + "password").Value, null);
		}
	}

	private static void UpdateLicense(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace xNamespace = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(xNamespace + "License")
			select (d))
		{
			item.Element(xNamespace + "Data").Value = method(item.Element(xNamespace + "Data").Value, null);
		}
	}

	private static void UpdateHostedHardware(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace xNamespace = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(xNamespace + "HostedHardware")
			select (d))
		{
			item.Element(xNamespace + "password").Value = method(item.Element(xNamespace + "password").Value, null);
		}
	}

	private static void UpdateSystemSettings(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace ns = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(ns + "SystemSettings")
			where d.Element(ns + "Name").Value == "AVHSPass" || d.Element(ns + "Name").Value == "ActivationCredentials" || d.Element(ns + "Name").Value == "SmtpPassword" || d.Element(ns + "Name").Value == "LicenseRsaKey" || d.Element(ns + "Name").Value == "MediaDBCertificate"
			select d)
		{
			item.Element(ns + "Setting").Value = method(item.Element(ns + "Setting").Value, null);
		}
	}

	private static void UpdateHardware(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace xNamespace = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(xNamespace + "Hardware")
			select (d))
		{
			byte[] arg = new Guid(item.Element(xNamespace + "IDHardware").Value).ToByteArray();
			item.Element(xNamespace + "EncryptedPassword").Value = method(item.Element(xNamespace + "EncryptedPassword").Value, arg);
			XElement xElement = item.Element(xNamespace + "Settings");
			XElement xElement2 = xElement.Element("properties");
			if (xElement2 == null)
			{
				xElement.Value = SettingsXmlTransformer.Instance.Transform(xElement.Value, (string setting) => method(setting, null));
				continue;
			}
			XElement root = XDocument.Parse(SettingsXmlTransformer.Instance.Transform(xElement2.ToString(), (string setting) => method(setting, null))).Root;
			xElement2.ReplaceWith(root);
		}
	}

	private static void UpdateRecordingStorage(XDocument doc, Func<string, byte[], string> method)
	{
		XNamespace xNamespace = ResolveNamespace(doc);
		foreach (XElement item in from d in doc.Descendants(xNamespace + "RecordingStorage")
			select (d))
		{
			item.Element(xNamespace + "encryption_password").Value = method(item.Element(xNamespace + "encryption_password").Value, null);
			item.Element(xNamespace + "encryption_password_new").Value = method(item.Element(xNamespace + "encryption_password_new").Value, null);
		}
	}

	private static XNamespace ResolveNamespace(XDocument doc)
	{
		XNamespace xNamespace = doc.Root.GetDefaultNamespace();
		if (xNamespace == XNamespace.None)
		{
			xNamespace = doc.Root.Elements().FirstOrDefault()?.GetDefaultNamespace();
		}
		return xNamespace;
	}
}
