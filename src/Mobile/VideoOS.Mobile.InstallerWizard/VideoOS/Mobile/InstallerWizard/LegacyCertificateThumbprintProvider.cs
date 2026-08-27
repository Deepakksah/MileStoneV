using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Mobile.Configuration;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.Helpers.Certificates;
using VideoOS.Mobile.InstallerWizard.Utilities;

namespace VideoOS.Mobile.InstallerWizard;

public class LegacyCertificateThumbprintProvider
{
	private readonly IFileService _configFileService;

	private ILogManager _installerLogger;

	private string _installationPath;

	private string _certHash;

	private IRegistryHelper _registryInstance;

	private const X509KeyStorageFlags DefaultX509KeyStorageFlags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet;

	public LegacyCertificateThumbprintProvider(IFileService configFileService, IRegistryProvider registryHelper, ILogManager logManager)
	{
		_installerLogger = logManager;
		_configFileService = configFileService;
		_registryInstance = registryHelper.GetRegistryInstance();
		ReadInitialValuesFromRegistry();
		if (_certHash == null)
		{
			string securedPortFromConfig = GetSecuredPortFromConfig();
			_certHash = Certificate.GetSslCertificateHashBindedOnPort(IPAddress.Any.ToString(), securedPortFromConfig);
		}
	}

	public LegacyEncryption GetMobileServerLegacyEncryption()
	{
		LegacyEncryption result = LegacyEncryption.Empty;
		if (!string.IsNullOrEmpty(_certHash))
		{
			result = LegacyEncryption.Encrypted(_certHash);
		}
		return result;
	}

	private void ReadInitialValuesFromRegistry()
	{
		if (_registryInstance.HasValue<string>("InstallationFolder", File.Exists))
		{
			_installationPath = _registryInstance.Read("InstallationFolder");
		}
		if (_registryInstance.HasValue("Certificate", (string hash) => !string.IsNullOrWhiteSpace(hash)))
		{
			_certHash = _registryInstance.Read("Certificate");
			_certHash = Encryption.GetValue(_certHash, Encryption.Operation.Decrypt);
			_certHash = Certificate.TryFindCertificateByHash(StoreName.My, StoreLocation.LocalMachine, _certHash)?.Thumbprint;
		}
	}

	private string GetSecuredPortFromConfig()
	{
		string securedPortFromRegistry = GetSecuredPortFromRegistry();
		_installerLogger.Write("Trying to find secured port in Config file...");
		string text = _registryInstance.Read("InstallationFolder");
		if (string.IsNullOrEmpty(text))
		{
			_installerLogger.Write("Unable to retrieve installation path from registry!", TraceEventType.Warning);
			return securedPortFromRegistry;
		}
		if (!File.Exists(text))
		{
			_installerLogger.Write("Unable to find the installation folder in the file system!", TraceEventType.Warning);
			return securedPortFromRegistry;
		}
		InternalConfigBase internalConfigBase = new InternalConfigBase(text);
		internalConfigBase.InitFromConfigurationFile();
		string text2 = internalConfigBase.HttpSecureMetaChannel.Address.Port.ToString();
		if (!PortHelper.IsPortValid(text2))
		{
			_installerLogger.Write("Port from config is invalid!", TraceEventType.Error);
			return securedPortFromRegistry;
		}
		return text2;
	}

	private string GetSecuredPortFromRegistry()
	{
		_installerLogger.Write("Trying to find secured port in registry...");
		string text = _registryInstance.Read("PortHttpSecured");
		if (string.IsNullOrEmpty(text) || !PortHelper.IsPortValid(text))
		{
			_installerLogger.Write("Unable to find secured port in registry.", TraceEventType.Warning);
			return string.Empty;
		}
		return text;
	}
}
