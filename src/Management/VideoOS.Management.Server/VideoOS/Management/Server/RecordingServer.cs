using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class RecordingServer
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int SynchronizationTime { get; set; }

	public DateTime LastModified { get; set; }

	public string HostName { get; set; }

	public bool Enabled { get; set; }

	public string TimeZoneName { get; set; }

	public string MulticastSettings { get; set; }

	public string DevicePackInfo { get; set; }

	public bool X64 { get; set; }

	public string PrimaryFailoverGroup { get; set; }

	public string SecondaryFailoverGroup { get; set; }

	public int FailoverPort { get; set; }

	public string FailoverHostName { get; set; }

	public string FailoverId { get; set; }

	public string ScanRanges { get; set; }

	public int ScanRangeWarning { get; set; }

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

	public bool ActivePublicAccessEnabled { get; set; }

	public string ActivePublicWebServerHostName { get; set; }

	public int ActivePublicWebServerPort { get; set; }

	public bool PublicAccessEnabled { get; set; }

	public string PublicWebServerHostName { get; set; }

	public int PublicWebServerPort { get; set; }

	public string MulticastServerAddress { get; set; }

	public bool ShutdownOnStorageFailure { get; set; }

	public string ActiveWebServerUriConfiguration
	{
		get
		{
			if (!ActivePublicAccessEnabled || string.IsNullOrEmpty(ActivePublicWebServerHostName))
			{
				return string.Empty;
			}
			if (!CertificateConfigured)
			{
				return UriHandling.CreateHttpUri(ActivePublicWebServerHostName, ActivePublicWebServerPort).AbsoluteUri;
			}
			return UriHandling.CreateHttpsUri(ActivePublicWebServerHostName, ActivePublicWebServerPort).AbsoluteUri;
		}
	}

	public CustomProperty[] CustomProperties { get; set; }

	public string Version { get; set; }

	public static RecordingServer CreateRecordingServer(IGeminiRequestContext requestContext, string name, string hostName, string webServerAddress, string timeZoneName, string scanRanges, string defaultStorageName, string defaultStoragePath, int? defaultStorageRetainMinutes, long? defaultStorageMaxSizeInMB, int? defaultFailoverUdpPort)
	{
		return CreateRecordingServer(requestContext, Guid.NewGuid().ToString(), name, hostName, webServerAddress, timeZoneName, scanRanges, defaultStorageName, defaultStoragePath, defaultStorageRetainMinutes, defaultStorageMaxSizeInMB, defaultFailoverUdpPort);
	}

	public static RecordingServer CreateRecordingServer(IGeminiRequestContext requestContext, string recordingServerId, string name, string hostName, string webServerAddress, string timeZoneName, string scanRanges, string defaultStorageName, string defaultStoragePath, int? defaultStorageRetainMinutes, long? defaultStorageMaxSizeInMB, int? defaultFailoverUdpPort)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(recordingServerId, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name");
		Validation.CheckHostName(hostName, "HostName");
		Validation.CheckWebAddress(webServerAddress, allowHttps: true, "WebServerUri");
		Validation.CheckStringNotNullOrWhiteSpace(timeZoneName, "TimeZoneName");
		Validation.CheckAddressIntervals(scanRanges, "ScanRanges", allowNullOrEmpty: true);
		Validation.CheckName(defaultStorageName, "DefaultStorageName");
		Validation.CheckPath(defaultStoragePath, "DefaultStoragePath");
		Validation.CheckHasValue(defaultStorageRetainMinutes, "DefaultStorageRetainMinutes");
		Validation.CheckRetainMinutes(defaultStorageRetainMinutes.Value, "DefaultStorageRetainMinutes");
		Validation.CheckHasValue(defaultStorageMaxSizeInMB, "DefaultStorageMaxSizeInMB");
		Validation.CheckMaxSize(defaultStorageMaxSizeInMB.Value, "DefaultStorageMaxSizeInMB");
		if (!defaultFailoverUdpPort.HasValue || defaultFailoverUdpPort.Value == 0)
		{
			defaultFailoverUdpPort = 11000;
		}
		Validation.CheckIsPositive(defaultFailoverUdpPort.Value, "DefaultFailoverUdpPort", allowNull: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		int recorderLicenseCount = requestContext.FeatureCacheManager.GetRecorderLicenseCount(requestContext);
		Guid sessionId = Guid.NewGuid();
		if (string.IsNullOrEmpty(scanRanges))
		{
			scanRanges = new AddressIntervals().ToXml();
		}
		RecordingServer result;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			if (recordingServerComponent.QueryEnabledRecordingServersCount() >= recorderLicenseCount)
			{
				throw new UnlicensedRecordersException();
			}
			Uri uri = new Uri(webServerAddress);
			bool isMulticastLicensed = requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "Multicast");
			string recordingServerId2 = recordingServerComponent.CreateRecordingServer(recordingServerId, name, hostName, uri.Host, uri.Port, timeZoneName, scanRanges, isMulticastLicensed, defaultFailoverUdpPort.Value);
			result = recordingServerComponent.QueryRecordingServer(recordingServerId2);
		}
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			RecordingStorage recordingStorage = new RecordingStorage
			{
				Id = Guid.NewGuid().ToString(),
				Name = defaultStorageName,
				Description = string.Empty,
				Path = defaultStoragePath,
				IsDefault = true,
				CreatedDate = DateTime.UtcNow,
				LastModified = DateTime.UtcNow,
				RetainMinutes = defaultStorageRetainMinutes.Value,
				MaxSize = defaultStorageMaxSizeInMB.Value,
				Encryption = EncryptionMethod.None,
				Signing = false,
				RecordingServerId = recordingServerId
			};
			recordingStorageComponent.CreateRecordingStorage(recordingStorage, sessionId);
			recordingStorageComponent.SetRecordingStorageAsDefault(recordingStorage.Id);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogEntryAuditLogger.AuditRecordingServerCreate(Guid.Parse(recordingServerId), name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "RecordingServer", recordingServerId, EventAction.Added, name);
		return result;
	}

	public static RecordingServer QueryRecordingServer(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(recordingServerId, "Id", allowNullOrEmpty: false);
		RecordingServer result = null;
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
			result = recordingServerComponent.QueryRecordingServer(recordingServerId);
		}
		return result;
	}

	public static bool RecordingServerExists(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(recordingServerId, "Id", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			return false;
		}
		using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
		return recordingServerComponent.QueryRecordingServer(recordingServerId) != null;
	}

	public static void UpdateRecordingServerValidation(IGeminiRequestContext requestContext, string recordingServerId, string name, string description, int? synchronizationTime)
	{
		Validation.CheckObjectId(recordingServerId, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		Validation.ValidateInt32(synchronizationTime, "SynchronizationTime", -1, 1439, allowNull: true);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerId);
	}

	public static void UpdateRecordingServerServiceConfigurableValidation(IGeminiRequestContext requestContext, string recordingServerId, bool? publicAccessEnabled, string publicWebserverHostName, int? publicWebserverPort, bool? shutdownOnStorageFailure, string multicastServerAddress)
	{
		Validation.CheckPublicWebAddress(publicWebserverHostName, "PublicWebserverHostName");
		Validation.ValidateInt32(publicWebserverPort, "PublicWebserverPort", 0, 65535, allowNull: true);
		if (true.Equals(publicAccessEnabled))
		{
			if (string.IsNullOrEmpty(publicWebserverHostName))
			{
				throw new ArgumentException("PublicWebAddress cannot be blank when PublicAccessEnabled is true", "PublicWebserverHostName");
			}
			if (publicWebserverPort == 0)
			{
				throw new ArgumentException("PublicWebPort cannot be 0 when PublicAccessEnabled is true", "PublicWebserverPort");
			}
		}
		Validation.ValidateMulticastIP(multicastServerAddress, "multicastServerAddress", allowNull: true);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerId);
	}

	public static RecordingServer UpdateRecordingServer(IGeminiRequestContext requestContext, string recordingServerId, string name, string description, int? synchronizationTime, CustomProperty[] customProperties)
	{
		UpdateRecordingServerValidation(requestContext, recordingServerId, name, description, synchronizationTime);
		RecordingServer recordingServer = null;
		Guid sessionId = Guid.NewGuid();
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerComponent.UpdateRecordingServer(recordingServerId, name, description, synchronizationTime, customProperties, sessionId);
			recordingServer = recordingServerComponent.QueryRecordingServer(recordingServerId);
		}
		Integration.FireConfigurationChanged(requestContext, "RecordingServer", recordingServer.Id, EventAction.Updated, recordingServer.Name);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.TryNotifyServerAsync();
		return recordingServer;
	}

	public static void UpdateRecordingServerServiceConfigurable(IGeminiRequestContext requestContext, string recordingServerId, bool? publicAccessEnabled, string publicWebAddress, int? publicWebPort, bool? shutdownOnStorageFailure, string multicastServerAddress)
	{
		UpdateRecordingServerServiceConfigurableValidation(requestContext, recordingServerId, publicAccessEnabled, publicWebAddress, publicWebPort, shutdownOnStorageFailure, multicastServerAddress);
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerComponent.UpdateRecordingServerServiceConfigurable(recordingServerId, publicAccessEnabled, publicWebAddress, publicWebPort, shutdownOnStorageFailure, multicastServerAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "RecordingServer", recordingServerId, EventAction.Updated, string.Empty);
		Integration.TryNotifyServerAsync();
	}

	public static void UpdateRecordingServerMulticastSettings(IGeminiRequestContext requestContext, string recordingServerId, string multicastSettings)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerId);
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerComponent.UpdateRecordingServerMulticastSettings(recordingServerId, multicastSettings);
		}
		Integration.FireConfigurationChanged(requestContext, "RecordingServerMulticast", recordingServerId, EventAction.Updated, string.Empty);
		Integration.TryNotifyServerAsync();
	}

	public static void UpdateRecordingServerFailoverSettings(IGeminiRequestContext requestContext, string recordingServerId, string primaryFailoverGroupId, string secondaryFailoverGroupId, int failoverPort)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerId);
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerComponent.UpdateRecordingServerFailoverSettings(recordingServerId, primaryFailoverGroupId, secondaryFailoverGroupId, failoverPort);
		}
		Integration.FireConfigurationChanged(requestContext, "RecordingServer", recordingServerId, EventAction.Updated, string.Empty);
		Integration.TryNotifyServerAsync();
	}

	public static RecordingServer[] QueryRecordingServersByIds(IGeminiRequestContext requestContext, string[] recordingServerIds)
	{
		RecordingServer[] recordingServers;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServers = recordingServerComponent.QueryRecordingServersByIds(recordingServerIds);
		}
		return FilterRecordingServers(requestContext, recordingServers);
	}

	public static string[] QueryRecordingServerIds(IGeminiRequestContext requestContext)
	{
		string[] recordingServerIds;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerIds = recordingServerComponent.QueryRecordingServerIds();
		}
		return FilterRecordingServers(requestContext, recordingServerIds);
	}

	public static string[] QueryRecordingServerIdsByServerGroupId(IGeminiRequestContext requestContext, string serverGroupId)
	{
		Validation.CheckObjectId(ref serverGroupId, "serverGroupId", allowNullOrEmpty: false);
		string[] recordingServerIds;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			recordingServerIds = recordingServerComponent.QueryRecordingServerIds(serverGroupId);
		}
		return FilterRecordingServers(requestContext, recordingServerIds);
	}

	private static RecordingServer[] FilterRecordingServers(IGeminiRequestContext requestContext, IEnumerable<RecordingServer> recordingServers)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServers.Select((RecordingServer r) => (r: r, Id: r.Id))).ToArray();
	}

	private static string[] FilterRecordingServers(IGeminiRequestContext requestContext, IEnumerable<string> recordingServerIds)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerIds.Select((string r) => (r, r))).ToArray();
	}

	public static string[] ReadDrivesOnRecorder(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			throw new RecordingServerNotFoundException();
		}
		return Integration.RecordingServer.ReadDrivesOnRecorder(recordingServerId);
	}

	public static string[] ReadDirectoriesOnRecorder(IGeminiRequestContext requestContext, string recordingServerId, string currentDir)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		if (string.IsNullOrEmpty(currentDir) || currentDir.IndexOfAny(Path.GetInvalidPathChars()) > -1)
		{
			throw new DirectoryInvalidException();
		}
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			throw new RecordingServerNotFoundException();
		}
		return Integration.RecordingServer.ReadDirectoriesOnRecorder(recordingServerId, currentDir);
	}

	public static void CreateDirectoryOnRecorder(IGeminiRequestContext requestContext, string recordingServerId, string newDirFullName)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		if (string.IsNullOrEmpty(newDirFullName) || newDirFullName.IndexOfAny(Path.GetInvalidPathChars()) > -1)
		{
			throw new DirectoryInvalidException();
		}
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerId))
		{
			throw new RecordingServerNotFoundException();
		}
		string text;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			text = recordingServerComponent.QueryRecordingServer(recordingServerId)?.Name;
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new RecordingServerNotFoundException();
		}
		Integration.RecordingServer.CreateDirectoryOnRecorder(recordingServerId, newDirFullName);
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditDirectoryCreatedOnRecorder(Guid.Parse(recordingServerId), text, newDirFullName, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}
