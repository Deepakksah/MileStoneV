using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using VideoOS.CertificateRegistration.Common.Properties;
using VideoOS.Utilities.Formatting;
using VideoOS.Utilities.Security;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.CertificateRegistration.Common;

public class HttpSysSettings : IHttpSysSettings
{
	private readonly List<ISslConfiguration> _apiSslConfigurations;

	private readonly Guid _appId;

	private readonly ushort _webApiPort;

	private readonly int _instancesCount;

	private bool _modified;

	private bool _encryptionEnabled;

	private byte[] _certificateHash;

	private SecurityIdentifier _serviceUserSid;

	private readonly string _serviceName;

	private readonly IEnumerable<string> _relativeUris;

	private readonly IHttpSysConfigFactory _httpSysConfigFactory;

	private readonly ISslConfigurationFactory _sslConfigurationFactory;

	public bool EncryptionEnabled
	{
		get
		{
			return _encryptionEnabled;
		}
		set
		{
			if (value != _encryptionEnabled)
			{
				_encryptionEnabled = value;
				_modified = true;
			}
		}
	}

	public byte[] CertificateHash
	{
		get
		{
			return _certificateHash;
		}
		set
		{
			if (_certificateHash != value)
			{
				_certificateHash = value;
				_modified = true;
			}
		}
	}

	public SecurityIdentifier ServiceUserSid
	{
		get
		{
			return _serviceUserSid;
		}
		set
		{
			if (_serviceUserSid != value)
			{
				_serviceUserSid = value;
				_modified = true;
			}
		}
	}

	public HttpSysSettings(Guid appId, int webApiPort)
		: this(appId, webApiPort, 1, null)
	{
	}

	public HttpSysSettings(Guid appId, int webApiPort, string serviceName)
		: this(appId, webApiPort, 1, serviceName)
	{
	}

	public HttpSysSettings(Guid appId, int webApiPort, int instancesCount)
		: this(appId, webApiPort, instancesCount, null)
	{
	}

	public HttpSysSettings(Guid appId, int webApiPort, int instancesCount, string serviceName)
		: this(appId, webApiPort, instancesCount, serviceName, null, HttpConfigFactory.Instance, SslConfigurationFactory.Instance)
	{
	}

	public HttpSysSettings(Guid appId, int webApiPort, int instancesCount, string serviceName, IEnumerable<string> relativeUris)
		: this(appId, webApiPort, instancesCount, serviceName, relativeUris, HttpConfigFactory.Instance, SslConfigurationFactory.Instance)
	{
	}

	public HttpSysSettings(Guid appId, int webApiPort, int instancesCount, string serviceName, IEnumerable<string> relativeUris, IHttpSysConfigFactory httpSysConfigFactory, ISslConfigurationFactory sslConfigurationFactory)
	{
		HttpSysSettings httpSysSettings = this;
		if (string.IsNullOrEmpty(serviceName) && appId == default(Guid))
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0} must no be empty", "appId")));
		}
		if (instancesCount <= 0)
		{
			throw new ArgumentOutOfRangeException("instancesCount", instancesCount, "Must have an instance count above 0.");
		}
		if (webApiPort < 0 || webApiPort > 65535)
		{
			throw new ArgumentOutOfRangeException("webApiPort", webApiPort, "Must be a valid port range.");
		}
		_appId = appId;
		_webApiPort = (ushort)webApiPort;
		_instancesCount = instancesCount;
		_serviceName = serviceName;
		_relativeUris = relativeUris;
		_httpSysConfigFactory = httpSysConfigFactory;
		_sslConfigurationFactory = sslConfigurationFactory;
		List<int> instancePorts = new List<int>();
		for (int i = 0; i < instancesCount; i++)
		{
			instancePorts.Add(_webApiPort + i);
		}
		using (IHttpSysConfig httpSysConfig = httpSysConfigFactory.CreateConfig())
		{
			_apiSslConfigurations = (from c in httpSysConfig.ReadSslConfigurations()
				where instancePorts.Contains(c.Port) || c.ApplicationId == appId
				select c).ToList();
		}
		ISslConfiguration sslConfiguration = _apiSslConfigurations.FirstOrDefault((ISslConfiguration c) => c.Port == httpSysSettings._webApiPort && object.Equals(c.Address, IPAddress.Any)) ?? _apiSslConfigurations.FirstOrDefault((ISslConfiguration c) => c.Port == httpSysSettings._webApiPort);
		_encryptionEnabled = sslConfiguration != null;
		_certificateHash = sslConfiguration?.GetCertificateHash();
	}

	public void SaveSettings()
	{
		SaveSettings(force: false);
	}

	public void SaveSettings(bool force)
	{
		if (!_modified && !force)
		{
			return;
		}
		using (IHttpSysConfig httpSysConfig = _httpSysConfigFactory.CreateConfig())
		{
			foreach (ISslConfiguration apiSslConfiguration in _apiSslConfigurations)
			{
				try
				{
					httpSysConfig.DeleteSslConfiguration(apiSslConfiguration);
				}
				catch (Win32Exception inner)
				{
					throw new ChangeEncryptionException(ChangeEncryptionTextResources.DeleteSslConfigurationFailed, inner);
				}
			}
			for (int i = 0; i < _instancesCount; i++)
			{
				ushort port = (ushort)(_webApiPort + i);
				if (EncryptionEnabled)
				{
					ISslConfiguration sslConfiguration = CreateDefaultConfigFromCertificate(IPAddress.Any, port, _appId, StoreName.My, CertificateHash);
					if (sslConfiguration == null)
					{
						throw new ChangeEncryptionException(ChangeEncryptionTextResources.CreateDefaultConfigFromCertificateFailed);
					}
					try
					{
						httpSysConfig.EnforceSslConfiguration(sslConfiguration);
					}
					catch (Win32Exception inner2)
					{
						throw new ChangeEncryptionException(ChangeEncryptionTextResources.EnforceSslConfigurationFailed, inner2);
					}
				}
				SecurityIdentifier securityIdentifier = ServiceUserSid;
				if (securityIdentifier == null)
				{
					try
					{
						securityIdentifier = FindServiceUser(i);
					}
					catch (Exception inner3)
					{
						throw new ChangeEncryptionException(ChangeEncryptionTextResources.FindServiceUserFailed, inner3);
					}
					if (securityIdentifier == null)
					{
						throw new ChangeEncryptionException(ChangeEncryptionTextResources.FindServiceUserSidFailed);
					}
				}
				try
				{
					UrlScheme urlScheme = (EncryptionEnabled ? UrlScheme.Https : UrlScheme.Http);
					IEnumerable<UrlPrefix> enumerable = CreateUrlPrefixes(urlScheme, port);
					UrlReservationAccessControlEntry urlReservationAccessControlEntry = new UrlReservationAccessControlEntry(securityIdentifier, allowListen: true, allowDelegate: false);
					foreach (UrlPrefix item in enumerable)
					{
						httpSysConfig.EnforceUrlReservation(item, urlReservationAccessControlEntry);
					}
				}
				catch (Win32Exception inner4)
				{
					throw new ChangeEncryptionException(ChangeEncryptionTextResources.EnforceUrlReservationFailed, inner4);
				}
			}
		}
		_modified = false;
	}

	private IEnumerable<UrlPrefix> CreateUrlPrefixes(UrlScheme urlScheme, ushort port)
	{
		List<string> list = new List<string>();
		if (_relativeUris != null && _relativeUris.Any())
		{
			list.AddRange(_relativeUris);
		}
		else
		{
			list.Add("/");
		}
		return list.Select((string x) => UrlPrefix.CreateStrongWildcard(urlScheme, port, x));
	}

	public void DeleteSettings()
	{
		using (IHttpSysConfig httpSysConfig = _httpSysConfigFactory.CreateConfig())
		{
			foreach (ISslConfiguration apiSslConfiguration in _apiSslConfigurations)
			{
				try
				{
					httpSysConfig.DeleteSslConfiguration(apiSslConfiguration);
				}
				catch (Win32Exception inner)
				{
					throw new ChangeEncryptionException(ChangeEncryptionTextResources.DeleteSslConfigurationFailed, inner);
				}
			}
			for (int i = 0; i < _instancesCount; i++)
			{
				ushort port = (ushort)(_webApiPort + i);
				try
				{
					UrlScheme urlScheme = (EncryptionEnabled ? UrlScheme.Https : UrlScheme.Http);
					foreach (UrlPrefix item in CreateUrlPrefixes(urlScheme, port))
					{
						httpSysConfig.DeleteUrlReservation(item);
					}
				}
				catch (Win32Exception inner2)
				{
					throw new ChangeEncryptionException(ChangeEncryptionTextResources.EnforceUrlReservationFailed, inner2);
				}
			}
		}
		_modified = false;
	}

	private ISslConfiguration CreateDefaultConfigFromCertificate(IPAddress address, ushort port, Guid applicationId, StoreName certificateStore, byte[] certificateHash)
	{
		return _sslConfigurationFactory.GetDefaultConfigurationFromCertificate(address, port, applicationId, certificateStore, certificateHash.AsString());
	}

	private SecurityIdentifier FindServiceUser(int instanceNumber)
	{
		if (string.IsNullOrEmpty(_serviceName))
		{
			return null;
		}
		_ = _serviceName;
		string text = Account.GetServiceLogOnUser((instanceNumber == 0) ? _serviceName : (_serviceName + " " + instanceNumber.ToString(CultureInfo.InvariantCulture)));
		if (text != null)
		{
			if (text.StartsWith(".\\", StringComparison.Ordinal))
			{
				text = Environment.MachineName + text.TrimStart('.');
			}
			return new NTAccount(text).Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
		}
		return null;
	}
}
