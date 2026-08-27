using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Installer.RecorderPlugins;

public class LegacyCertificateThumbprintProvider
{
	private readonly IFileService _configFileService;

	private readonly IOemResources _oemResources;

	private XmlDocument _doc;

	public LegacyCertificateThumbprintProvider(IFileService configFileService, IOemResources oemResource)
	{
		_configFileService = configFileService;
		_oemResources = oemResource;
	}

	public LegacyEncryption GetImageServerLegacyEncryption()
	{
		if (_doc == null)
		{
			_doc = OpenConfiguration();
		}
		if (_doc == null)
		{
			return LegacyEncryption.Empty;
		}
		LegacyEncryption empty = LegacyEncryption.Empty;
		empty = ConvertSettingsToThumbprint(RecorderConfigEncryptionReader.ReadEncryptionSettings(_doc));
		if (!empty.HasValue)
		{
			empty = ConvertSettingsToThumbprint(RecorderConfigEncryptionReader.ReadOldConfiguration(_doc));
		}
		return empty;
	}

	public LegacyEncryption GetRecordingServerLegacyEncryption()
	{
		if (_doc == null)
		{
			_doc = OpenConfiguration();
		}
		if (_doc == null)
		{
			return LegacyEncryption.Empty;
		}
		ushort num = 0;
		try
		{
			string text = _doc["recorderconfig"]?["webapi"]?["port"]?.InnerText;
			if (text == null)
			{
				return LegacyEncryption.Empty;
			}
			num = Convert.ToUInt16(text, CultureInfo.InvariantCulture);
		}
		catch (FormatException)
		{
			return LegacyEncryption.Empty;
		}
		catch (OverflowException)
		{
			return LegacyEncryption.Empty;
		}
		byte[] certificateHash = new HttpSysSettings(WellKnownAppIds.RecorderApiService, num, 1).CertificateHash;
		if (certificateHash == null || certificateHash.Length == 0)
		{
			return LegacyEncryption.Unencrypted;
		}
		return LegacyEncryption.Encrypted(AsString(certificateHash));
	}

	private static string AsString(byte[] input)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		return string.Join("", input.Select((byte b) => b.ToString("X2", CultureInfo.InvariantCulture)));
	}

	private XmlDocument OpenConfiguration()
	{
		if (!_configFileService.Exists(_oemResources.Recorder.ConfigFilePath))
		{
			return null;
		}
		XmlDocument xmlDocument = new XmlDocument();
		using Stream inStream = _configFileService.Open(_oemResources.Recorder.ConfigFilePath, FileMode.Open, FileAccess.Read);
		xmlDocument.Load(inStream);
		return xmlDocument;
	}

	private static LegacyEncryption ConvertSettingsToThumbprint(CertificateEncryptionSettings settings)
	{
		if (settings == null)
		{
			return LegacyEncryption.Empty;
		}
		if (!settings.EncryptionEnabled)
		{
			return LegacyEncryption.Unencrypted;
		}
		if (string.IsNullOrEmpty(settings.CertificateHash))
		{
			return LegacyEncryption.Empty;
		}
		return LegacyEncryption.Encrypted(settings.CertificateHash);
	}
}
