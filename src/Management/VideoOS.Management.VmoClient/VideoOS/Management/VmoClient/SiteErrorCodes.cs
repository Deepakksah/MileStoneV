namespace VideoOS.Management.VmoClient;

public static class SiteErrorCodes
{
	public const int SiteHomeSyncErrorConnection = 101;

	public const int SiteHomeSyncErrorVersion = 102;

	public const int SiteHomeSyncErrorServerInformation = 103;

	public const int SiteSlaveSyncErrorSlaveConnection = 201;

	public const int SiteSlaveSyncErrorHomeConnection = 202;

	public const int SiteSlaveSyncErrorHomeVersion = 203;

	public const int SiteSlaveSyncErrorSlaveVersion = 204;

	public const int SiteSlaveSyncErrorVersionMismatch = 205;

	public const int SiteSlaveSyncErrorHomeServerInformation = 206;

	public const int SiteSlaveSyncErrorSlaveServerInformation = 207;

	public const int SiteSlaveSyncErrorSlaveAccountOnHome = 208;

	public const int SiteSlaveSyncErrorHomeAccountOnSlave = 209;

	public const int SiteSlaveSyncErrorNoMaster = 210;

	public const int SiteMasterSyncErrorMasterConnection = 301;

	public const int SiteMasterSyncErrorHomeConnection = 302;

	public const int SiteMasterSyncErrorHomeVersion = 303;

	public const int SiteMasterSyncErrorMasterVersion = 304;

	public const int SiteMasterSyncErrorVersionMismatch = 305;

	public const int SiteMasterSyncErrorHomeServerInformation = 306;

	public const int SiteMasterSyncErrorMasterServerInformation = 307;

	public const int SiteMasterSyncErrorMasterAccountOnHome = 308;

	public const int SiteMasterSyncErrorHomeAccountOnMaster = 309;
}
