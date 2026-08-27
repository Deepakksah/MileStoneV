using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Security;
using System.Xml;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public interface ISystemInfo
{
	string UserName { get; set; }

	SecureString Password { get; set; }

	AuthenticationType AuthenticationType { get; }

	string Url { get; set; }

	Uri Uri { get; }

	string Hostname { get; }

	int Port { get; }

	bool IsConnected { get; }

	bool Invalidated { get; }

	Exception LastException { get; set; }

	ServerTypes ServerType { get; }

	CameraCollection Cameras { get; }

	RecorderCollection Recorders { get; }

	MicrophoneCollection Microphones { get; }

	SpeakerCollection Speakers { get; }

	UserDefinedEventCollection Events { get; }

	OutputDeviceCollection OutputDevices { get; }

	MatrixMonitorCollection MatrixMonitors { get; }

	IDictionary<Guid, IMetadataDevice> MetadataDevices { get; }

	InternalList<Guid> SlaveIds { get; }

	bool SlavesPopulated { get; }

	string MacAddress { get; set; }

	Dictionary<string, string> Properties { get; }

	HttpStatusCode LastStatusCode { get; set; }

	string Token { get; }

	UserContext UserContext { get; }

	NetworkCredential Credentials { get; }

	bool MasterOnly { get; set; }

	string Name { get; set; }

	Guid Id { get; }

	ServerId ServerId { get; }

	ILoginServer LoginServer { get; }

	LoginSettings LoginSettings { get; set; }

	bool Populated { get; }

	bool PopulatedHasBeenTried { get; }

	IConfigurationInfo ConfigurationInfo { get; }

	DateTime LastHardwareUpdate { get; }

	bool ProductSupportOfKind(Guid kind);

	void AddSingleCameraConfig(Guid recorderGuid, XmlDocument doc);

	void AddSingleMicrophoneConfig(Guid recorderGuid, XmlDocument doc);

	string GetMIPServerType();

	void UpdateServerId(Guid id);

	void RebuildDeviceCollections();

	void RefreshRecorder(FQID recorderFqid);

	bool Populate();

	bool Login();

	void SetConnectRetry(bool autoConnect);

	bool IsLoggedIn();

	bool ServerConnectionCheck();

	bool Logout();

	void Disconnect();

	void AddToDisposableObjects(IDisposable obj);

	Collection<IDisposable> DisposableObjects();

	IChange[] QueryChanges(DateTime changesFrom);

	IRecorderInfo QueryRecorderInfo(Guid recorderId);

	void UpdateLastHardwareUpdateIfNewer(DateTime newLastModified);

	void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName);
}
