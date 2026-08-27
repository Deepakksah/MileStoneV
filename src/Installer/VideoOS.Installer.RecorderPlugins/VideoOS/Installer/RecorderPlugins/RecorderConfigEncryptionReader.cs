using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Formatting;

namespace VideoOS.Installer.RecorderPlugins;

public static class RecorderConfigEncryptionReader
{
	public static CertificateEncryptionSettings ReadOldConfiguration(IXPathNavigable doc)
	{
		if (doc == null)
		{
			throw new ArgumentNullException("doc");
		}
		XPathNavigator xPathNavigator = doc.CreateNavigator().SelectSingleNode("/recorderconfig/webserver/certificate");
		if (xPathNavigator == null)
		{
			return new CertificateEncryptionSettings
			{
				EncryptionEnabled = true,
				CertificateHash = string.Empty
			};
		}
		bool encryptionEnabled = bool.Parse(xPathNavigator.SelectSingleNode("@enabled").Value);
		string value = xPathNavigator.SelectSingleNode("@findType").Value;
		X509FindType findType = (X509FindType)Enum.Parse(typeof(X509FindType), value, ignoreCase: true);
		string value2 = xPathNavigator.SelectSingleNode("@findValue").Value;
		string value3 = xPathNavigator.SelectSingleNode("@storeLocation").Value;
		CertificateData certificateData = FindCertificateFromOldSearchCriteria((StoreLocation)Enum.Parse(typeof(StoreLocation), value3, ignoreCase: true), findType, value2);
		return new CertificateEncryptionSettings
		{
			EncryptionEnabled = encryptionEnabled,
			CertificateHash = ((certificateData == null) ? string.Empty : certificateData.CertificateHash.AsString())
		};
	}

	private static CertificateData FindCertificateFromOldSearchCriteria(StoreLocation storeLocation, X509FindType findType, string findValue)
	{
		using X509Store x509Store = new X509Store(StoreName.My, storeLocation);
		x509Store.Open(OpenFlags.OpenExistingOnly);
		return (from c in (from X509Certificate2 i in x509Store.Certificates.Find(findType, findValue, validOnly: false)
				select new CertificateData(i)).FilterServerValidCertificates()
			orderby c.NotBefore descending
			select c).FirstOrDefault();
	}

	public static CertificateEncryptionSettings ReadEncryptionSettings(IXPathNavigable doc)
	{
		if (doc == null)
		{
			throw new ArgumentNullException("doc");
		}
		XPathNavigator xPathNavigator = doc.CreateNavigator().SelectSingleNode("/recorderconfig/webserver/encryption");
		if (xPathNavigator == null)
		{
			return null;
		}
		bool encryptionEnabled = bool.Parse(xPathNavigator.SelectSingleNode("@enabled").Value);
		string value = xPathNavigator.SelectSingleNode("@certificateHash").Value;
		return new CertificateEncryptionSettings
		{
			EncryptionEnabled = encryptionEnabled,
			CertificateHash = value
		};
	}
}
