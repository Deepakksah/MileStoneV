using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Management.Server;

public sealed class RecorderData
{
	public Guid Id { get; set; }

	public Guid ServiceId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int SynchronizationTime { get; set; }

	public string HostName { get; set; }

	public bool CertificateConfigured { get; set; }

	public string WebServerHostName { get; set; }

	public int WebServerPort { get; set; }

	public string WebServerUri
	{
		get
		{
			if (!CertificateConfigured)
			{
				return UriHandling.CreateHttpUri(WebServerHostName, WebServerPort).AbsoluteUri;
			}
			return UriHandling.CreateHttpsUri(WebServerHostName, WebServerPort).AbsoluteUri;
		}
	}

	public bool PublicAccessEnabled { get; set; }

	public string PublicWebServerHostName { get; set; }

	public int PublicWebServerPort { get; set; }

	public string WebServerUriConfiguration
	{
		get
		{
			if (!PublicAccessEnabled || string.IsNullOrEmpty(PublicWebServerHostName))
			{
				return string.Empty;
			}
			if (!CertificateConfigured)
			{
				return UriHandling.CreateHttpUri(PublicWebServerHostName, PublicWebServerPort).AbsoluteUri;
			}
			return UriHandling.CreateHttpsUri(PublicWebServerHostName, PublicWebServerPort).AbsoluteUri;
		}
	}

	public string TimeZoneName { get; set; }

	public DateTime LastModified { get; set; }

	public static IEnumerable<RecorderData> QueryEnabledRecorders(IGeminiRequestContext requestContext, Guid[] recorderIds)
	{
		using IRecorderDataComponent recorderDataComponent = requestContext.CreateRecorderDataComponent();
		return recorderDataComponent.QueryEnabledRecorders(recorderIds);
	}

	public static RecorderData QueryActiveRecorder(IGeminiRequestContext requestContext, Guid recorderId)
	{
		using IRecorderDataComponent recorderDataComponent = requestContext.CreateRecorderDataComponent();
		return recorderDataComponent.QueryActiveRecorder(recorderId);
	}

	public static IEnumerable<RecorderModifiedData> QueryRecordersChangedAfter(IGeminiRequestContext requestContext, DateTime changesFrom)
	{
		using IRecorderDataComponent recorderDataComponent = requestContext.CreateRecorderDataComponent();
		return recorderDataComponent.QueryRecordersChangedAfter(changesFrom);
	}
}
