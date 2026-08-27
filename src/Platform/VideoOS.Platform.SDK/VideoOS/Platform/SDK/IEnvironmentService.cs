using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Media;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK;

public interface IEnvironmentService
{
	IEnvironmentProperties Properties { get; }

	void Init();

	void Reset();

	void AddServer(LoginSettings loginSettings, bool masterOnly);

	void AddServer(bool secureOnly, Item siteItem, CredentialCache credentialCache, bool masterOnly);

	void AddServer(bool secureOnly, Uri uri, CredentialCache credentialCache, bool masterOnly);

	void AddServer(bool secureOnly, Uri uri, NetworkCredential networkCredential, bool masterOnly);

	bool IsLoggedIn(Uri serverUri);

	bool IsLoggedInToAnyServer();

	bool LoadConfiguration(Uri serverUri);

	Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache);

	Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType);

	Item LoadSiteItem(bool secureOnly, Uri uri);

	bool IsServerConnected(Uri uri);

	void Login(Uri serverUri, bool masterOnly);

	void Login(Uri serverUri, bool masterOnly, Guid integrationId, string integrationName, string version, string manufacturerName);

	void Logout();

	void Logout(Uri serverUri);

	void ReloadConfiguration(FQID serverFQID);

	void RemoveAllServers();

	void RemoveServer(Guid id);

	void RemoveServer(Uri uri);

	List<Item> GetDisabledDevices(Guid serverId);

	void InitializeUIFramework(IDictionary<string, Color> brandColors);

	void LogError(string where, string text);
}
