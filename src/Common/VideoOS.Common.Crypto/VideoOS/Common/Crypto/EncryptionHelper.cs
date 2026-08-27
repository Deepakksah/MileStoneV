using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace VideoOS.Common.Crypto;

public sealed class EncryptionHelper
{
	private EncryptionAlgorithms _encryptionAlgorithm;

	private SymmetricAlgorithm cryptoServiceProvider;

	private CompressionTypes compression;

	public EncryptionAlgorithms EncryptionAlgorithm
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

	public CompressionTypes Compression
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

	public EncryptionHelper(EncryptionAlgorithms encryptionAlgorithm)
	{
		compression = CompressionTypes.None;
		InitCryptoServiceProvider(encryptionAlgorithm);
	}

	public EncryptionHelper(string symmetricAlgorithmConfigurationXml)
	{
		compression = CompressionTypes.None;
		InitFromSymmetricAlgorithmConfigurationXml(symmetricAlgorithmConfigurationXml);
	}

	private void InitCryptoServiceProvider(EncryptionAlgorithms encryptionAlgorithm)
	{
		_encryptionAlgorithm = encryptionAlgorithm;
		switch (_encryptionAlgorithm)
		{
		case EncryptionAlgorithms.DES:
			cryptoServiceProvider = new DESCryptoServiceProvider();
			break;
		case EncryptionAlgorithms.RC2:
			cryptoServiceProvider = new RC2CryptoServiceProvider();
			break;
		case EncryptionAlgorithms.Rijndael:
			cryptoServiceProvider = new RijndaelManaged();
			break;
		case EncryptionAlgorithms.TripleDES:
			cryptoServiceProvider = new TripleDESCryptoServiceProvider();
			break;
		default:
			throw new NotSupportedException("The specified encryption algorithm is not supported");
		}
	}

	public byte[] EncryptStringData(string inputDataString, out string encryptionIV)
	{
		byte[] encryptionIV2;
		byte[] result = EncryptStringData(inputDataString, out encryptionIV2);
		encryptionIV = Convert.ToBase64String(encryptionIV2);
		return result;
	}

	public byte[] EncryptStringData(string inputDataString, out byte[] encryptionIV)
	{
		cryptoServiceProvider.GenerateIV();
		encryptionIV = cryptoServiceProvider.IV;
		return EncryptStringData(inputDataString);
	}

	public byte[] EncryptStringData(string inputDataString, string password, string saltString)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(saltString);
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes);
		cryptoServiceProvider.IV = rfc2898DeriveBytes.GetBytes(cryptoServiceProvider.BlockSize / 8);
		cryptoServiceProvider.Key = rfc2898DeriveBytes.GetBytes(cryptoServiceProvider.LegalKeySizes[0].MaxSize / 8);
		return EncryptStringData(inputDataString);
	}

	public byte[] EncryptStringData(string inputDataString)
	{
		ICryptoTransform transform = cryptoServiceProvider.CreateEncryptor();
		MemoryStream memoryStream;
		if (compression == CompressionTypes.None)
		{
			memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(inputDataString));
		}
		else
		{
			memoryStream = new MemoryStream();
			byte[] bytes = Encoding.Unicode.GetBytes(inputDataString);
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

	public string DecryptStringData(byte[] encrypted, string password, string saltString)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(saltString);
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, bytes);
		cryptoServiceProvider.IV = rfc2898DeriveBytes.GetBytes(cryptoServiceProvider.BlockSize / 8);
		cryptoServiceProvider.Key = rfc2898DeriveBytes.GetBytes(cryptoServiceProvider.LegalKeySizes[0].MaxSize / 8);
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
		if (compression != CompressionTypes.None)
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
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
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

	public static string CreateSymmetricAlgorithmConfigurationXml(EncryptionAlgorithms encryptionAlgorithm, byte[] key, byte[] iv, CompressionTypes compression)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
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
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
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
		EncryptionAlgorithms encryptionAlgorithm = (EncryptionAlgorithms)Enum.Parse(value: xmlDocument.SelectSingleNode("/SymmetricAlgorithmConfiguration/Algorithm").InnerText, enumType: typeof(EncryptionAlgorithms));
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
				compression = (CompressionTypes)Enum.Parse(typeof(CompressionTypes), xmlNode3.InnerText);
			}
			catch (OutOfMemoryException)
			{
				throw;
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
			CompressionTypes.None => s, 
			CompressionTypes.Deflate => new DeflateStream(s, mode, leaveOpen: true), 
			_ => s, 
		};
	}
}
