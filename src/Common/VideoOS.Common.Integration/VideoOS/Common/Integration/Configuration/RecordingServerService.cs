using System;
using System.Data;
using System.Diagnostics;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.Configuration;

public sealed class RecordingServerService : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RecordingServerServiceRow _datarow;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.pkid);
		}
	}

	public string HostName
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.host_name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.host_name = value;
		}
	}

	public bool CertificateConfigured
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.certificate_configured;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.certificate_configured = value;
		}
	}

	public string WebServerHostName
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.webserver_hostname;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.webserver_hostname = value;
		}
	}

	public int WebServerPort
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.webserver_port;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.webserver_port = value;
		}
	}

	public Uri WebServerUri
	{
		get
		{
			if (!CertificateConfigured)
			{
				return UriHandling.CreateHttpUri(WebServerHostName, WebServerPort);
			}
			return UriHandling.CreateHttpsUri(WebServerHostName, WebServerPort);
		}
	}

	public string ScanRanges
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.scan_ranges;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.scan_ranges = value;
		}
	}

	public string TimeZoneName
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.time_zone_name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.time_zone_name = value;
		}
	}

	public ushort FailoverPort
	{
		[DebuggerStepThrough]
		get
		{
			return Convert.ToUInt16(_datarow.failover_udp_port);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.failover_udp_port = value;
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.last_modified;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.last_modified = value;
		}
	}

	public string DevicePackInfo
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.devicepack_info;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.devicepack_info = value;
		}
	}

	public bool X64
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.x64;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.x64 = value;
		}
	}

	public RecordingServerServiceConfigurable ServiceConfiguration => _configurationManager.RecordingServerServiceConfigurable[ID];

	public Uri WebServerUriConfiguration
	{
		get
		{
			RecordingServerServiceConfigurable serviceConfiguration = ServiceConfiguration;
			if (!serviceConfiguration.PublicAccessEnabled || string.IsNullOrEmpty(serviceConfiguration.PublicWebserverHostName) || serviceConfiguration.PublicWebserverPort == 0)
			{
				return null;
			}
			if (!CertificateConfigured)
			{
				return UriHandling.CreateHttpUri(serviceConfiguration.PublicWebserverHostName, serviceConfiguration.PublicWebserverPort);
			}
			return UriHandling.CreateHttpsUri(serviceConfiguration.PublicWebserverHostName, serviceConfiguration.PublicWebserverPort);
		}
	}

	public string WebServerUriConfigurationText
	{
		get
		{
			Uri webServerUriConfiguration = WebServerUriConfiguration;
			if (!(webServerUriConfiguration != null))
			{
				return string.Empty;
			}
			return webServerUriConfiguration.AbsoluteUri;
		}
	}

	public RecordingServerService(ConfigurationManager configurationManager, ConfigurationDataSet.RecordingServerServiceRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}

	public Uri CalculateWebServerUri(bool externalRequest)
	{
		Uri uri = null;
		if (externalRequest)
		{
			uri = WebServerUriConfiguration;
		}
		if (!(uri == null))
		{
			return uri;
		}
		return WebServerUri;
	}
}
