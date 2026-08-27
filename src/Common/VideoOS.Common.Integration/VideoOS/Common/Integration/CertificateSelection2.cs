using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Utilities.Formatting;

namespace VideoOS.Common.Integration;

public class CertificateSelection2
{
	public static CertificateSelection2 NoSelection { get; } = new CertificateSelection2
	{
		CertificateHash = new byte[0]
	};

	public byte[] CertificateHash { get; private set; }

	public static CertificateSelection2 FromCertificate(X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		return new CertificateSelection2
		{
			CertificateHash = certificate.GetCertHash()
		};
	}

	public static CertificateSelection2 FromCertificateData(CertificateData certificateData)
	{
		if (certificateData == null)
		{
			throw new ArgumentNullException("certificateData");
		}
		byte[] array = new byte[certificateData.CertificateHash.Length];
		Buffer.BlockCopy(certificateData.CertificateHash, 0, array, 0, array.Length);
		return new CertificateSelection2
		{
			CertificateHash = array
		};
	}

	public static CertificateSelection2 FromCertificateHash(byte[] certificateHash)
	{
		if (certificateHash == null)
		{
			throw new ArgumentNullException("certificateHash");
		}
		if (certificateHash.Length == 0)
		{
			return NoSelection;
		}
		return new CertificateSelection2
		{
			CertificateHash = certificateHash
		};
	}

	public static CertificateSelection2 FromCertificateHashString(string certificateHash)
	{
		if (certificateHash == null)
		{
			throw new ArgumentNullException("certificateHash");
		}
		if (string.IsNullOrEmpty(certificateHash))
		{
			return NoSelection;
		}
		return new CertificateSelection2
		{
			CertificateHash = certificateHash.ParseAsBytes()
		};
	}

	public override int GetHashCode()
	{
		int num = 3;
		byte[] certificateHash = CertificateHash;
		foreach (byte b in certificateHash)
		{
			num = (num * 397) ^ b.GetHashCode();
		}
		return num;
	}

	public static bool operator ==(CertificateSelection2 left, CertificateSelection2 right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(CertificateSelection2 left, CertificateSelection2 right)
	{
		return !(left == right);
	}

	public bool Equals(CertificateSelection2 other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return CertificateHash.SequenceEqual(other.CertificateHash);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is CertificateSelection2 other))
		{
			return false;
		}
		return Equals(other);
	}

	public CertificateSelection2 Clone()
	{
		return (CertificateSelection2)MemberwiseClone();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		xmlWriterSettings.NewLineHandling = NewLineHandling.None;
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			xmlWriter.WriteStartDocument(standalone: true);
			xmlWriter.WriteStartElement("CertificateSelection2");
			xmlWriter.WriteElementString("CertificateHash", CertificateHash.AsString());
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
		}
		return stringBuilder.ToString();
	}

	public static CertificateSelection2 FromXml(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		if (string.IsNullOrEmpty(xml))
		{
			throw new ArgumentException("Xml cannot be empty.", "xml");
		}
		CertificateSelection2 certificateSelection = new CertificateSelection2();
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		using StringReader input = new StringReader(xml);
		using XmlReader reader = XmlReader.Create(input, xmlReaderSettings);
		XPathNavigator xPathNavigator = new XPathDocument(reader).CreateNavigator();
		string text = "CertificateSelection2/CertificateHash";
		XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode(text);
		if (xPathNavigator2 == null)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Missing element '{0}'.", text), "xml");
		}
		certificateSelection.CertificateHash = xPathNavigator2.InnerXml.ParseAsBytes();
		return certificateSelection;
	}
}
