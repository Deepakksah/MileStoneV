using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace VideoOS.Installer.RecorderPlugins;

public static class CertificateEncryptionSettingsSerializer
{
	private static XmlSerializer Serializer { get; } = new XmlSerializer(typeof(CertificateEncryptionSettings));

	public static string SerializeAsString(this CertificateEncryptionSettings value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		Serializer.Serialize(stringWriter, value);
		return stringWriter.ToString();
	}

	public static CertificateEncryptionSettings Deserialize(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		using MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(value));
		return (CertificateEncryptionSettings)Serializer.Deserialize(stream);
	}
}
