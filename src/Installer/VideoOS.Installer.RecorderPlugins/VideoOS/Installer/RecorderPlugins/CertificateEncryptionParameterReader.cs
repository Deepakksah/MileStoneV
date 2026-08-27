using System;
using System.Collections.Generic;

namespace VideoOS.Installer.RecorderPlugins;

public static class CertificateEncryptionParameterReader
{
	public static CertificateEncryptionSettings GetEncryptionSettings(string encryptionSettingParameterName, Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		CertificateEncryptionSettings result = null;
		if (parameters.TryGetValue(encryptionSettingParameterName, out var value) && !string.IsNullOrEmpty(value))
		{
			result = CertificateEncryptionSettingsSerializer.Deserialize(value);
		}
		return result;
	}
}
