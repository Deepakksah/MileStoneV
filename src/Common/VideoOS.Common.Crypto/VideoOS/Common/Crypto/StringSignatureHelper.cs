using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Crypto;

public sealed class StringSignatureHelper
{
	private AsymmetricAlgorithm algorithmFactory;

	private AsymmetricAlgorithms algorithm;

	public string PrivateKeyXml => algorithmFactory.ToXmlString(includePrivateParameters: true);

	public string PublicKeyXml => algorithmFactory.ToXmlString(includePrivateParameters: false);

	public StringSignatureHelper(AsymmetricAlgorithms algorithm)
	{
		InitializeAlgorithmFactory(algorithm);
	}

	public StringSignatureHelper(string keyXml)
	{
		if (keyXml.Contains("<RSAKeyValue>"))
		{
			InitializeAlgorithmFactory(AsymmetricAlgorithms.RSA);
		}
		else
		{
			if (!keyXml.Contains("<DSAKeyValue>"))
			{
				throw new NotSupportedException("The asymmetric algorithm key xml is not supported.");
			}
			InitializeAlgorithmFactory(AsymmetricAlgorithms.DSA);
		}
		algorithmFactory.FromXmlString(keyXml);
	}

	private void InitializeAlgorithmFactory(AsymmetricAlgorithms algorithm)
	{
		switch (algorithm)
		{
		case AsymmetricAlgorithms.RSA:
			algorithmFactory = RSA.Create();
			break;
		case AsymmetricAlgorithms.DSA:
			algorithmFactory = DSA.Create();
			break;
		default:
			throw new NotSupportedException("The specified algorithm is not supported");
		}
		this.algorithm = algorithm;
	}

	private AsymmetricSignatureFormatter CreateFormatter()
	{
		return algorithm switch
		{
			AsymmetricAlgorithms.RSA => new RSAPKCS1SignatureFormatter(algorithmFactory), 
			AsymmetricAlgorithms.DSA => new DSASignatureFormatter(algorithmFactory), 
			_ => throw new NotSupportedException("The algorithm is not supported."), 
		};
	}

	private AsymmetricSignatureDeformatter CreateDeformatter()
	{
		return algorithm switch
		{
			AsymmetricAlgorithms.RSA => new RSAPKCS1SignatureDeformatter(algorithmFactory), 
			AsymmetricAlgorithms.DSA => new DSASignatureDeformatter(algorithmFactory), 
			_ => throw new NotSupportedException("The algorithm is not supported."), 
		};
	}

	public string CreateSignatureString(string data)
	{
		byte[] rgbHash = CreateHash(data);
		AsymmetricSignatureFormatter asymmetricSignatureFormatter = CreateFormatter();
		asymmetricSignatureFormatter.SetHashAlgorithm("SHA1");
		return Convert.ToBase64String(asymmetricSignatureFormatter.CreateSignature(rgbHash));
	}

	public bool VerifySignature(string data, string signatureString)
	{
		byte[] rgbHash = CreateHash(data);
		byte[] rgbSignature = Convert.FromBase64String(signatureString);
		AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = CreateDeformatter();
		asymmetricSignatureDeformatter.SetHashAlgorithm("SHA1");
		return asymmetricSignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
	}

	private byte[] CreateHash(string data)
	{
		SHA1 sHA = SHA1.Create();
		byte[] bytes = Encoding.Unicode.GetBytes(data);
		return sHA.ComputeHash(bytes);
	}
}
