using System;

namespace VideoOS.Common.Integration.Configuration;

public interface ISettingsXmlTransformer
{
	string Decrypt(string settingsXml, Func<string, string> decryptFunc);

	string Encrypt(string settingsXml, Func<string, string> encryptFunc);

	string Transform(string settingsXml, Func<string, string> transformFunc);
}
