using System;
using System.Collections.Generic;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Platform;

public interface ISDKConfiguration
{
	Dictionary<string, ISystemInfo> SystemInfoList { get; }

	bool Login(Uri uri, bool loadSubSitesAfterwards = true);

	bool Logout(Uri serverUri);

	void LogoutAllServers();

	bool IsLoggedIn(Uri serverUri);

	bool IsLoggedInAny();

	void AddConfiguration(LoginSettings loginSettings, bool loadSubSitesAfterwards);

	bool CheckServerConnection(Uri uri);

	Exception LastException(Uri serverUri);

	bool TryLoadConfiguration(Uri serverUri);

	void RefreshServer(FQID serverFQID);

	List<Item> GetDisabledDevices(Guid serverId);

	ISystemInfo GetSystemInfo(Uri serverUri);

	void RemoveServer(Guid id);

	LoginSettings RemoveServer(Uri uri);

	void RemoveAllServers();

	void RegisterIntegration(Uri serverUri, Guid integrationId, string integrationName, string version, string manufacturerName);
}
