using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.CertificateRegistration.Common;

public class HttpSysSettingsFactory : IHttpSysSettingsFactory
{
	private readonly Guid _appId;

	private readonly int _webApiPort;

	private readonly int _instancesCount = 1;

	private readonly string _serviceName;

	private readonly IEnumerable<string> _relativeUris;

	public HttpSysSettingsFactory(Guid appId, int webApiPort, string serviceName, IEnumerable<string> relativeUris)
		: this(appId, webApiPort, serviceName)
	{
		if (relativeUris != null && !relativeUris.Any())
		{
			throw new ArgumentException("relativeUris cannot be an empty collection, use null instead");
		}
		_relativeUris = relativeUris;
	}

	public HttpSysSettingsFactory(Guid appId, int webApiPort, int instancesCount, string serviceName)
		: this(appId, webApiPort, serviceName)
	{
		_instancesCount = instancesCount;
	}

	public HttpSysSettingsFactory(Guid appId, int webApiPort, string serviceName)
	{
		_appId = appId;
		if (webApiPort <= 0 || webApiPort > 65535)
		{
			throw new ArgumentOutOfRangeException("webApiPort", webApiPort, "Must be in a valid port range.");
		}
		_webApiPort = webApiPort;
		_serviceName = serviceName ?? throw new ArgumentNullException("serviceName");
	}

	public HttpSysSettingsFactory()
	{
	}

	public IHttpSysSettings Create()
	{
		return new HttpSysSettings(_appId, _webApiPort, _instancesCount, _serviceName, _relativeUris);
	}

	public IHttpSysSettings Create(Guid addId, int webApiPort)
	{
		if (webApiPort <= 0 || webApiPort > 65535)
		{
			throw new ArgumentOutOfRangeException("webApiPort", webApiPort, "Must be in a valid port range.");
		}
		return new HttpSysSettings(addId, webApiPort);
	}
}
