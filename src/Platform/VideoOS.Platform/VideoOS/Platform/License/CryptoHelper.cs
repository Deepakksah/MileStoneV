using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.License;

public class CryptoHelper
{
	private EncryptionAlgorithm _encryptionAlgorithm;

	private SymmetricAlgorithm cryptoServiceProvider;

	private CompressionType compression;

	public EncryptionAlgorithm EncryptionAlgorithm
	{
		[DebuggerStepThrough]
		get
		{
			return _encryptionAlgorithm;
		}
		[DebuggerStepThrough]
		set
		{
			_encryptionAlgorithm = value;
		}
	}

	public SymmetricAlgorithm CryptoServiceProvider
	{
		[DebuggerStepThrough]
		get
		{
			return cryptoServiceProvider;
		}
		[DebuggerStepThrough]
		set
		{
			cryptoServiceProvider = value;
		}
	}

	public CompressionType Compression
	{
		[DebuggerStepThrough]
		get
		{
			return compression;
		}
		[DebuggerStepThrough]
		set
		{
			compression = value;
		}
	}

	public CryptoHelper(EncryptionAlgorithm encryptionAlgorithm)
	{
		compression = CompressionType.None;
		InitCryptoServiceProvider(encryptionAlgorithm);
	}

	public CryptoHelper(string symmetricAlgorithmConfigurationXml)
	{
		compression = CompressionType.None;
		InitFromSymmetricAlgorithmConfigurationXml(symmetricAlgorithmConfigurationXml);
	}

	private void InitCryptoServiceProvider(EncryptionAlgorithm encryptionAlgorithm)
	{
		_encryptionAlgorithm = encryptionAlgorithm;
		switch (_encryptionAlgorithm)
		{
		case EncryptionAlgorithm.DES:
			cryptoServiceProvider = DES.Create();
			break;
		case EncryptionAlgorithm.RC2:
			cryptoServiceProvider = RC2.Create();
			break;
		case EncryptionAlgorithm.Rijndael:
			throw new ArgumentException("Rijndeal encryption is obsolete");
		case EncryptionAlgorithm.TripleDES:
			cryptoServiceProvider = TripleDES.Create();
			break;
		default:
			throw new NotSupportedException("The specified encryption algorithm is not supported");
		}
	}

	public byte[] EncryptStringData(string inputData, out string encryptionIV)
	{
		byte[] encryptionIV2;
		byte[] result = EncryptStringData(inputData, out encryptionIV2);
		encryptionIV = Convert.ToBase64String(encryptionIV2);
		return result;
	}

	public byte[] EncryptStringData(string inputData, out byte[] encryptionIV)
	{
		cryptoServiceProvider.GenerateIV();
		encryptionIV = cryptoServiceProvider.IV;
		return EncryptStringData(inputData);
	}

	public byte[] EncryptStringData(string inputData)
	{
		ICryptoTransform transform = cryptoServiceProvider.CreateEncryptor();
		MemoryStream memoryStream;
		if (compression == CompressionType.None)
		{
			memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(inputData));
		}
		else
		{
			memoryStream = new MemoryStream();
			byte[] bytes = Encoding.Unicode.GetBytes(inputData);
			Stream stream = CreateCompressionStream(memoryStream, CompressionMode.Compress);
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
			stream.Close();
		}
		using MemoryStream memoryStream2 = new MemoryStream();
		using CryptoStream cryptoStream = new CryptoStream(memoryStream2, transform, CryptoStreamMode.Write);
		cryptoStream.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
		cryptoStream.FlushFinalBlock();
		cryptoStream.Close();
		return memoryStream2.ToArray();
	}

	public string DecryptStringData(byte[] encrypted, string encryptionIV)
	{
		cryptoServiceProvider.IV = Convert.FromBase64String(encryptionIV);
		return DecryptStringData(encrypted);
	}

	public string DecryptStringData(byte[] encrypted, byte[] encryptionIV)
	{
		cryptoServiceProvider.IV = encryptionIV;
		return DecryptStringData(encrypted);
	}

	public string DecryptStringData(byte[] encrypted)
	{
		ICryptoTransform transform = cryptoServiceProvider.CreateDecryptor();
		using MemoryStream stream = new MemoryStream(encrypted);
		using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		byte[] array = new byte[65536];
		MemoryStream memoryStream = new MemoryStream();
		int count;
		while ((count = cryptoStream.Read(array, 0, array.Length)) > 0)
		{
			memoryStream.Write(array, 0, count);
		}
		if (compression != CompressionType.None)
		{
			memoryStream.Position = 0L;
			using Stream stream2 = CreateCompressionStream(memoryStream, CompressionMode.Decompress);
			memoryStream = new MemoryStream();
			while ((count = stream2.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, count);
			}
			stream2.Close();
		}
		return Encoding.Unicode.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
	}

	public string GetSecretKeyAsXml()
	{
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.None;
		xmlTextWriter.WriteStartElement("SecretKey");
		xmlTextWriter.WriteElementString("Key", Convert.ToBase64String(cryptoServiceProvider.Key));
		xmlTextWriter.WriteElementString("IV", Convert.ToBase64String(cryptoServiceProvider.IV));
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringWriter.ToString().Substring(stringWriter.Encoding.GetPreamble().Length);
	}

	public void SetSecretKeyFromXml(string secretKeyXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(secretKeyXml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/SecretKey/Key");
		cryptoServiceProvider.Key = Convert.FromBase64String(xmlNode.InnerText);
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/SecretKey/IV");
		cryptoServiceProvider.IV = Convert.FromBase64String(xmlNode2.InnerText);
	}

	public static string CreateSymmetricAlgorithmConfigurationXml(EncryptionAlgorithm encryptionAlgorithm, byte[] key, byte[] iv, CompressionType compression)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder));
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("SymmetricAlgorithmConfiguration");
		xmlTextWriter.WriteElementString("Algorithm", encryptionAlgorithm.ToString());
		xmlTextWriter.WriteElementString("Key", Convert.ToBase64String(key));
		xmlTextWriter.WriteElementString("IV", Convert.ToBase64String(iv));
		xmlTextWriter.WriteElementString("Compression", compression.ToString());
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public string GetSymmetricAlgorithmConfigurationXml()
	{
		return GetSymmetricAlgorithmConfigurationXml(includeIV: false);
	}

	public string GetSymmetricAlgorithmConfigurationXml(bool includeIV)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder));
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("SymmetricAlgorithmConfiguration");
		xmlTextWriter.WriteElementString("Algorithm", _encryptionAlgorithm.ToString());
		xmlTextWriter.WriteElementString("Key", Convert.ToBase64String(cryptoServiceProvider.Key));
		if (includeIV)
		{
			xmlTextWriter.WriteElementString("IV", Convert.ToBase64String(cryptoServiceProvider.IV));
		}
		xmlTextWriter.WriteElementString("Compression", compression.ToString());
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndDocument();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	private void InitFromSymmetricAlgorithmConfigurationXml(string symmetricAlgorithmConfigurationXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(symmetricAlgorithmConfigurationXml);
		EncryptionAlgorithm encryptionAlgorithm = (EncryptionAlgorithm)Enum.Parse(value: xmlDocument.SelectSingleNode("/SymmetricAlgorithmConfiguration/Algorithm").InnerText, enumType: typeof(EncryptionAlgorithm));
		InitCryptoServiceProvider(encryptionAlgorithm);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("/SymmetricAlgorithmConfiguration/Key");
		cryptoServiceProvider.Key = Convert.FromBase64String(xmlNode.InnerText);
		XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/SymmetricAlgorithmConfiguration/IV");
		if (xmlNode2 != null && xmlNode2.InnerText.Length > 0)
		{
			cryptoServiceProvider.IV = Convert.FromBase64String(xmlNode2.InnerText);
		}
		XmlNode xmlNode3 = xmlDocument.SelectSingleNode("/SymmetricAlgorithmConfiguration/Compression");
		if (xmlNode3 != null && xmlNode3.InnerText.Length > 0)
		{
			try
			{
				compression = (CompressionType)Enum.Parse(typeof(CompressionType), xmlNode3.InnerText);
			}
			catch (Exception)
			{
				throw new UnknownCompressionTypeException("An unsupported compression type was specified. Type: " + xmlNode3.InnerText);
			}
		}
	}

	private Stream CreateCompressionStream(Stream s, CompressionMode mode)
	{
		return compression switch
		{
			CompressionType.None => s, 
			CompressionType.Deflate => new DeflateStream(s, mode, leaveOpen: true), 
			_ => s, 
		};
	}
}
