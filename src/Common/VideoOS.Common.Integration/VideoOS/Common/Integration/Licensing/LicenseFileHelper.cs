using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration.Licensing;

public sealed class LicenseFileHelper : ILicenseFileHelper
{
	public enum ContentType
	{
		Unknown,
		Request,
		Activated,
		Initial
	}

	public enum Compression
	{
		NoCompression,
		Deflate
	}

	public enum Version
	{
		One = 1
	}

	private static readonly byte[] FilePattern = new byte[8] { 88, 80, 114, 111, 116, 101, 99, 116 };

	public string GetContentFromInitialLicenseFile(byte[] licenseFile)
	{
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(licenseFile));
		if (binaryReader.ReadBytes(FilePattern.Length).SequenceEqual(FilePattern))
		{
			if (!((ContentType)binaryReader.ReadByte()/*cast due to constrained. prefix*/).Equals(ContentType.Initial))
			{
				throw new NotSupportedException("This method only supports initial licenses");
			}
			binaryReader.ReadUInt16();
			binaryReader.ReadByte();
			int count = binaryReader.ReadInt32();
			byte[] value = binaryReader.ReadBytes(count);
			byte[] bytes = Decompress(EncryptionFactory.CreateSymmetric().DecryptSequence(value));
			return Encoding.Unicode.GetString(bytes);
		}
		throw new NotSupportedException();
	}

	public string GetContentFromActivatedLicenseFile(byte[] licenseFile, IAsymmetricEncryption asymmetricEncryption)
	{
		if (asymmetricEncryption == null)
		{
			throw new ArgumentNullException("asymmetricEncryption");
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(licenseFile));
		if (binaryReader.ReadBytes(FilePattern.Length).SequenceEqual(FilePattern))
		{
			if (!((ContentType)binaryReader.ReadByte()/*cast due to constrained. prefix*/).Equals(ContentType.Activated))
			{
				throw new NotSupportedException("This method only supports activated licenses");
			}
			binaryReader.ReadUInt16();
			binaryReader.ReadByte();
			int count = binaryReader.ReadInt32();
			byte[] data = binaryReader.ReadBytes(count);
			int count2 = binaryReader.ReadInt32();
			byte[] value = binaryReader.ReadBytes(count2);
			byte[] bytes = Decompress(EncryptionFactory.CreateSymmetric(Convert.ToBase64String(asymmetricEncryption.Decrypt(data))).DecryptSequence(value));
			return Encoding.Unicode.GetString(bytes);
		}
		throw new NotSupportedException();
	}

	public byte[] CreateLicenseRequestFile(string requestJsonPayload, string lrqPublicKey, string sitePublicKey)
	{
		if (string.IsNullOrEmpty(lrqPublicKey))
		{
			throw new ArgumentException("'" + lrqPublicKey + "' can't be null or empty", "lrqPublicKey");
		}
		if (string.IsNullOrEmpty(sitePublicKey))
		{
			throw new ArgumentException("'" + sitePublicKey + "' can't be null or empty", "sitePublicKey");
		}
		using RSACng rSACng = new RSACng();
		rSACng.FromXmlString(lrqPublicKey);
		using IAsymmetricEncryption asymmetricEncryption = EncryptionFactory.CreateAsymmetricForLicense(rSACng.Key);
		using AesCng aesCng = new AesCng();
		aesCng.GenerateKey();
		ISymmetricEncryption symmetricEncryption = EncryptionFactory.CreateSymmetric(Convert.ToBase64String(aesCng.Key));
		byte value = 1;
		byte[] array = asymmetricEncryption.Encrypt(symmetricEncryption.Key);
		byte[] bytes = Encoding.Unicode.GetBytes(sitePublicKey);
		byte[] value2 = Compress(Encoding.Unicode.GetBytes(requestJsonPayload));
		byte[] array2 = symmetricEncryption.EncryptSequence(value2);
		using MemoryStream memoryStream = new MemoryStream();
		using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(FilePattern);
		binaryWriter.Write(value);
		binaryWriter.Write((ushort)1);
		binaryWriter.Write((byte)1);
		binaryWriter.Write(array.Length);
		binaryWriter.Write(array);
		binaryWriter.Write(bytes.Length);
		binaryWriter.Write(bytes);
		binaryWriter.Write(array2.Length);
		binaryWriter.Write(array2);
		binaryWriter.Flush();
		return memoryStream.ToArray();
	}

	public static ContentType DetermineLicenseType(byte[] licenseFile)
	{
		ContentType result = ContentType.Unknown;
		using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(licenseFile)))
		{
			if (binaryReader.ReadBytes(FilePattern.Length).SequenceEqual(FilePattern))
			{
				result = (ContentType)binaryReader.ReadByte();
			}
		}
		return result;
	}

	public static byte[] Decompress(byte[] data)
	{
		MemoryStream stream = new MemoryStream(data);
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
		{
			deflateStream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}

	public static byte[] Compress(byte[] data)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionLevel.Optimal))
		{
			deflateStream.Write(data, 0, data.Length);
		}
		return memoryStream.ToArray();
	}
}
