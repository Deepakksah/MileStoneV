using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public static class XmlSerializationHelper
{
	public static T FromXml<T>(string xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringReader textReader = new StringReader(xml);
		return (T)xmlSerializer.Deserialize(textReader);
	}

	public static string ToXml<T>(T @object)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, string.Empty);
			using MemoryStream memoryStream = new MemoryStream();
			xmlSerializer.Serialize(memoryStream, @object, xmlSerializerNamespaces);
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static T LoadXml<T>(string fileName)
	{
		string xml = LoadXmlString(fileName);
		try
		{
			return FromXml<T>(xml);
		}
		catch (InvalidOperationException inner)
		{
			throw new LoadXmlException("Failed to deserialize xml", inner);
		}
	}

	public static string LoadXmlString(string fileName)
	{
		try
		{
			return File.ReadAllText(fileName);
		}
		catch (ArgumentNullException inner)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner);
		}
		catch (ArgumentException inner2)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner2);
		}
		catch (PathTooLongException inner3)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner3);
		}
		catch (DirectoryNotFoundException inner4)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner4);
		}
		catch (UnauthorizedAccessException inner5)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner5);
		}
		catch (FileNotFoundException inner6)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner6);
		}
		catch (IOException inner7)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner7);
		}
		catch (NotSupportedException inner8)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner8);
		}
		catch (SecurityException inner9)
		{
			throw new LoadXmlException("Failed to load xml from disk", inner9);
		}
	}

	public static void SaveXml<T>(T @object, string fileName)
	{
		string xml;
		try
		{
			xml = ToXml(@object);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new SaveXmlException("Failed to serialize xml", inner);
		}
		SaveXmlString(fileName, xml);
	}

	public static void SaveXmlString(string fileName, string xml)
	{
		try
		{
			File.WriteAllText(fileName, xml);
		}
		catch (ArgumentNullException inner)
		{
			throw new SaveXmlException("Failed write xml to disk", inner);
		}
		catch (ArgumentException inner2)
		{
			throw new SaveXmlException("Failed write xml to disk", inner2);
		}
		catch (PathTooLongException inner3)
		{
			throw new SaveXmlException("Failed write xml to disk", inner3);
		}
		catch (DirectoryNotFoundException inner4)
		{
			throw new SaveXmlException("Failed write xml to disk", inner4);
		}
		catch (UnauthorizedAccessException inner5)
		{
			throw new SaveXmlException("Failed write xml to disk", inner5);
		}
		catch (FileNotFoundException inner6)
		{
			throw new SaveXmlException("Failed write xml to disk", inner6);
		}
		catch (IOException inner7)
		{
			throw new SaveXmlException("Failed write xml to disk", inner7);
		}
		catch (NotSupportedException inner8)
		{
			throw new SaveXmlException("Failed write xml to disk", inner8);
		}
		catch (SecurityException inner9)
		{
			throw new SaveXmlException("Failed write xml to disk", inner9);
		}
	}

	public static string Encrypt(string s, ISymmetricEncryption symmetricEncryption)
	{
		return symmetricEncryption.EncryptToBase64String(s);
	}

	public static string Decrypt(string s, ISymmetricEncryption symmetricEncryption)
	{
		return symmetricEncryption.DecryptFromBase64String(s);
	}

	public static string LegacyDecrypt(string s)
	{
		byte[] encrypted = Convert.FromBase64String(s);
		return CreateEncryptionHelper().DecryptStringData(encrypted);
	}

	private static EncryptionHelper CreateEncryptionHelper()
	{
		return new EncryptionHelper("<?xml version=\"1.0\" encoding=\"utf-16\"?><SymmetricAlgorithmConfiguration><Algorithm>DES</Algorithm><Key>WPuvO2koUlE=</Key><IV>pkWP5B3gBpY=</IV><Compression>None</Compression></SymmetricAlgorithmConfiguration>");
	}
}
