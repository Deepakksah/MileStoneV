using System;
using System.Xml.Linq;

namespace VideoOS.Common.Integration.Configuration;

public static class XmlEncryptionHelper
{
	public static string EncryptConfigurationDataSet(string xml, Func<string, byte[], string> encryptMethod)
	{
		if (string.IsNullOrEmpty(xml))
		{
			return xml;
		}
		return UpdateEncryption(XmlTransformationFactory.DiffGramCreate(), xml, encryptMethod);
	}

	public static string DecryptConfigurationDataSet(string encryptedXml, Func<string, byte[], string> decryptMethod)
	{
		if (string.IsNullOrEmpty(encryptedXml))
		{
			return encryptedXml;
		}
		return UpdateEncryption(XmlTransformationFactory.DiffGramCreate(), encryptedXml, decryptMethod);
	}

	public static string EncryptXml(string type, string xml, Func<string, byte[], string> encryptMethod)
	{
		if (string.IsNullOrEmpty(xml))
		{
			return xml;
		}
		return UpdateEncryptionFromType(type, xml, encryptMethod);
	}

	public static string DecryptXml(string type, string encryptedXml, Func<string, byte[], string> decryptMethod)
	{
		if (string.IsNullOrEmpty(encryptedXml))
		{
			return encryptedXml;
		}
		return UpdateEncryptionFromType(type, encryptedXml, decryptMethod);
	}

	private static string UpdateEncryptionFromType(string type, string data, Func<string, byte[], string> method)
	{
		return UpdateEncryption(XmlTransformationFactory.Create(type), data, method);
	}

	private static string UpdateEncryption(IXmlTransformation updater, string data, Func<string, byte[], string> method)
	{
		XDocument xDocument = XDocument.Parse(data);
		if (xDocument.Root == null)
		{
			return data;
		}
		updater?.Transform(xDocument, method);
		return xDocument.ToString();
	}
}
