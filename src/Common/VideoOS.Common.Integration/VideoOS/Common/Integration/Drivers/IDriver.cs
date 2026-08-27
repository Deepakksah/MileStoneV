using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriver : IDisposable
{
	Guid Id { get; }

	IDriverDevice[] Devices { get; }

	Uri Uri { get; }

	string UserName { get; }

	string Password { get; }

	string ProductId { get; }

	string SettingsAsXml { get; }

	string EventConfigurationSettingsAsXml { get; }

	string DefinitionAsXml { get; }

	string HardwareInformation { get; }

	IDeviceMetaDataLookup MetaDataLookup { get; set; }

	IDriverProxyInstanceManager DriverProxyInstanceManager { get; set; }

	bool InitializeHardwareAndDeviceSettingsSupported { get; }

	int DriverNumber { get; }

	event EventHandler<DriverEventArgs> HardwareDriverEvent;

	void Load(Guid hardwareId, Uri uri, string userName, string password, string driverProxyName, string customDriverData, string productId, string definitionXml, string hardwareSettingsXml, string eventSettingsXml, IEnumerable<IDeviceSetupInfo> deviceSetupInfoCollection);

	IDriverDevice GetDevice(Guid deviceId);

	IDriverDevice GetDeviceByHwid(DeviceHwid deviceHwid);

	IDriverDevice[] GetDevicesByType(string deviceType);

	void SetCredentials(string userName, string password);

	void SetAddress(Uri uri);

	void UpdateSettings(string settingsXml);

	bool ApplyEventConfigurationSettings(string xml);

	void Start();

	void Stop();

	string GetHardwareDefinitionXml();

	bool ApplyAllHardwareAndDeviceSettings(string settingsXml);

	bool InitializeHardwareAndDeviceSettings(string settingsXml);

	bool SetXmlCache(string key, string propertiesXml, string definitionXml);

	void EnableSettingsUpdates();

	void DisableSettingsUpdates();

	void ChangePasswordOnSpecificUser(string newPassword, string targetUserName);

	string GetUserList();

	void CreateUser(string newUserName, string newPassword, HardwareUserLevel newUserLevel);

	void DeleteUser(string deleteUserName);

	string GetNetworkConfiguration();

	void SetNetworkConfiguration(string newIPAddress, string newMask, string newGateway);

	void SetupOperationalState(string newUserName, string newPassword, int? securePort);

	string StartFirmwareUpgrade(string firmwareFileName);

	FirmwareUpgradeStatus GetFirmwareUpgradeStatus(string sessionId);

	bool CheckExternalRecorderRegistration(string externalRecorderAddress);
}
