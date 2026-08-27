using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace VideoOS.Common.Crypto;

public sealed class XmlSignatureHelper
{
	public class RSAPKCS1SHA384SignatureDescription : SignatureDescription
	{
		static RSAPKCS1SHA384SignatureDescription()
		{
			CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA384SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384");
		}

		public RSAPKCS1SHA384SignatureDescription()
		{
			base.KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
			base.DigestAlgorithm = typeof(SHA384CryptoServiceProvider).FullName;
			base.FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
			base.DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;
		}

		public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter obj = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(base.DeformatterAlgorithm);
			obj.SetKey(key);
			obj.SetHashAlgorithm("SHA384");
			return obj;
		}

		public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter obj = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(base.FormatterAlgorithm);
			obj.SetKey(key);
			obj.SetHashAlgorithm("SHA384");
			return obj;
		}
	}

	public class RSAPKCS1SHA512SignatureDescription : SignatureDescription
	{
		static RSAPKCS1SHA512SignatureDescription()
		{
			CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA512SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512");
		}

		public RSAPKCS1SHA512SignatureDescription()
		{
			base.KeyAlgorithm = typeof(RSACryptoServiceProvider).FullName;
			base.DigestAlgorithm = typeof(SHA512CryptoServiceProvider).FullName;
			base.FormatterAlgorithm = typeof(RSAPKCS1SignatureFormatter).FullName;
			base.DeformatterAlgorithm = typeof(RSAPKCS1SignatureDeformatter).FullName;
		}

		public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter obj = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(base.DeformatterAlgorithm);
			obj.SetKey(key);
			obj.SetHashAlgorithm("SHA512");
			return obj;
		}

		public override AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter obj = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(base.FormatterAlgorithm);
			obj.SetKey(key);
			obj.SetHashAlgorithm("SHA512");
			return obj;
		}
	}

	private AsymmetricAlgorithm algorithmFactory;

	private string signatureMethod;

	public int KeySize => algorithmFactory.KeySize;

	public string SignatureMethod => signatureMethod;

	public string PrivateKeyXml => algorithmFactory.ToXmlString(includePrivateParameters: true);

	public string PublicKeyXml => algorithmFactory.ToXmlString(includePrivateParameters: false);

	public XmlSignatureHelper(AsymmetricAlgorithms algorithm)
		: this(algorithm, 0, null)
	{
	}

	public XmlSignatureHelper(AsymmetricAlgorithms algorithm, int keySize, string signatureMethod)
	{
		switch (algorithm)
		{
		case AsymmetricAlgorithms.RSA:
			algorithmFactory = ((keySize > 0) ? RSA.Create(keySize) : RSA.Create());
			break;
		case AsymmetricAlgorithms.DSA:
			algorithmFactory = ((keySize > 0) ? DSA.Create(keySize) : DSA.Create());
			break;
		default:
			throw new NotSupportedException("The specified algorithm is not supported");
		}
		this.signatureMethod = signatureMethod;
	}

	public XmlSignatureHelper(string keyXml)
		: this(keyXml, null)
	{
	}

	public XmlSignatureHelper(string keyXml, string signatureMethod)
	{
		this.signatureMethod = signatureMethod;
		algorithmFactory = CreateAlgorithmFromXml(keyXml);
	}

	private AsymmetricAlgorithm GetPrivateKeyAlgorithm()
	{
		return CreateAlgorithmFromXml(PrivateKeyXml);
	}

	private AsymmetricAlgorithm GetPublicKeyAlgorithm()
	{
		return CreateAlgorithmFromXml(PublicKeyXml);
	}

	private AsymmetricAlgorithm CreateAlgorithmFromXml(string keyXml)
	{
		if (keyXml == null)
		{
			throw new ArgumentNullException("keyXml");
		}
		AsymmetricAlgorithm asymmetricAlgorithm;
		if (keyXml.Contains("<RSAKeyValue>"))
		{
			asymmetricAlgorithm = RSA.Create();
			asymmetricAlgorithm.FromXmlString(keyXml);
		}
		else
		{
			if (!keyXml.Contains("<DSAKeyValue>"))
			{
				throw new NotSupportedException("The asymmetric algorithm key xml is not supported.");
			}
			asymmetricAlgorithm = DSA.Create();
			asymmetricAlgorithm.FromXmlString(keyXml);
		}
		return asymmetricAlgorithm;
	}

	public string SignXmlString(string inputXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(inputXml);
		SignXmlDocument(xmlDocument);
		return xmlDocument.OuterXml;
	}

	public void SignXmlDocument(XmlDocument xmlDocument)
	{
		if (xmlDocument == null)
		{
			throw new ArgumentNullException("xmlDocument");
		}
		AsymmetricAlgorithm privateKeyAlgorithm = GetPrivateKeyAlgorithm();
		try
		{
			XmlNode xmlNode = xmlDocument.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
			xmlNode?.ParentNode.RemoveChild(xmlNode);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		SignedXml signedXml = new SignedXml(xmlDocument);
		signedXml.SigningKey = privateKeyAlgorithm;
		signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
		if (signatureMethod != null)
		{
			signedXml.SignedInfo.SignatureMethod = signatureMethod;
		}
		Reference reference = new Reference(string.Empty);
		reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
		signedXml.AddReference(reference);
		signedXml.ComputeSignature();
		XmlElement xml = signedXml.GetXml();
		xmlDocument.DocumentElement.AppendChild(xml);
	}

	public void VerifyXmlString(string inputXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(inputXml);
		VerifyXmlDocument(xmlDocument);
	}

	public void VerifyXmlDocument(XmlDocument xmlDocument)
	{
		AsymmetricAlgorithm publicKeyAlgorithm = GetPublicKeyAlgorithm();
		SignedXml signedXml = new SignedXml(xmlDocument);
		try
		{
			XmlNode xmlNode = xmlDocument.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
			signedXml.LoadXml((XmlElement)xmlNode);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			throw new SecurityException("Error: no signature found.");
		}
		if (!signedXml.CheckSignature(publicKeyAlgorithm))
		{
			throw new SecurityException("FAILED: Signature invalid.");
		}
	}
}
