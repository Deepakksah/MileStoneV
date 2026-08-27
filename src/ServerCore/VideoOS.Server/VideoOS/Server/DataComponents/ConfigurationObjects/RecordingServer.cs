using System;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class RecordingServer
{
	public DateTime LastModified { get; set; }

	public Guid RecorderId { get; set; }

	public Guid ServiceId { get; set; }

	public RecordingServerServiceType Type { get; set; }

	public bool Active { get; set; }

	public bool Enabled { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int SynchronizationTime { get; set; }

	public bool CertificateConfigured { get; set; }

	public string WebServerHostName { get; set; }

	public int WebServerPort { get; set; }

	public bool PublicAccessEnabled { get; set; }

	public string PublicWebServerHostName { get; set; }

	public int PublicWebServerPort { get; set; }

	public string HostName { get; set; }

	public string ScanRanges { get; set; }

	public string TimeZoneName { get; set; }

	public Guid FailoverPrimaryGroupId { get; set; }

	public Guid FailoverSecondaryGroupId { get; set; }

	public ushort FailoverUdpPort { get; set; }

	public string MulticastSettings { get; set; }

	public string DevicePackInfo { get; set; }

	public bool X64 { get; set; }

	public int DatabasePort { get; set; }

	public string Version { get; set; }

	public static RecordingServer QueryActiveRecordingServer(Guid recorderId)
	{
		using RecordingServerComponent recordingServerComponent = new RecordingServerComponent();
		return recordingServerComponent.QueryRecordingServerActive(recorderId);
	}

	public static Tuple<bool, bool> CreateOrUpdateRecorder(Guid recorderId, Guid serviceId, RecordingServerServiceType serviceType, string name, string hostName, Uri webServerUri, string scanRanges, string timeZoneName, string devicePackInfo, ushort failoverServerPort, int databasePort, Guid failoverId, Version version, bool isMulticastLicensed)
	{
		using RecordingServerComponent recordingServerComponent = new RecordingServerComponent();
		return recordingServerComponent.CreateOrUpdateRecorder(recorderId, serviceId, serviceType, name, hostName, webServerUri, scanRanges, timeZoneName, devicePackInfo, failoverServerPort, databasePort, failoverId, version, isMulticastLicensed);
	}
}
