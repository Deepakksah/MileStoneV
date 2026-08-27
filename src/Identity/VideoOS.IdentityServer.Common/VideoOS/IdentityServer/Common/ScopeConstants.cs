namespace VideoOS.IdentityServer.Common;

public static class ScopeConstants
{
	public const string ManagementScope = "management";

	public const string RecorderScope = "recorder";

	public const string ManagementServerScope = "managementserver";

	public const string DataCollectorScope = "datacollector";

	public const string IdpManagementScope = "idp_management";

	public const string IdpLogEventScope = "idp_log_event";

	public const string FailoverResponseServerScope = "failoverresponseserver";

	public const string MediaDbScope = "mediadb";

	public const string VPSScope = "vps";

	public const string IncidentManagerServerScope = "incidentmanagerserver";

	public const string FailoverServerScope = "failover";

	public const string ServerLogsScope = "serverlogs";

	public const string OpenIdScope = "openid";

	public const string ProfileScope = "profile";

	public const string OfflineAccessScope = "offline_access";

	public static readonly string[] ServerServiceScopes = new string[6] { "recorder", "idp_management", "idp_log_event", "managementserver", "failover", "serverlogs" };

	public static readonly string[] RecorderServiceScopes = new string[4] { "management", "mediadb", "vps", "serverlogs" };

	public static readonly string[] FailoverServiceScopes = new string[2] { "managementserver", "failoverresponseserver" };

	public static readonly string[] WebManagementClientScopes = new string[4] { "managementserver", "openid", "profile", "offline_access" };
}
