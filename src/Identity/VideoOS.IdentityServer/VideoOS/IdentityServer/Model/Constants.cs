namespace VideoOS.IdentityServer.Model;

public static class Constants
{
	public static class Scopes
	{
		public const string ClientRegistrationScope = "write:client";

		public const string IdpManagementScope = "idp_management";

		public const string IdpLogEventScope = "idp_log_event";

		public const string ManagementScope = "management";

		public const string RecorderScope = "recorder";

		public const string DataCollectorScope = "datacollector";

		public const string ManagementServerScope = "managementserver";

		public const string FailoverResponseServerScope = "failoverresponseserver";

		public const string MediaDbScope = "mediadb";

		public const string VPSScope = "vps";

		public const string IncidentManagerServerScope = "incidentmanagerserver";

		public const string FailoverServerScope = "failover";

		public const string ScimScope = "scim";

		public const string ServerLogsScope = "serverlogs";
	}

	public static class IdentityResouces
	{
		public const string ProviderProfile = "provider";
	}

	public static class ApiMessages
	{
		public const string UpdateOfPasswordFieldsNotAllowed = "Not allowed to modify password fields for a user with external provider";
	}

	public static class ClaimTypes
	{
		public const string VmsProviderKey = "vms_provider_key";
	}

	public const int SecretLength = 64;

	public const string WinAuthClientId = "winauthclient";

	public const string IdpManagementPolicyName = "IdpManagementPolicy";

	public const string IdpAuditEventPolicyName = "IdpAuditEventPolicy";

	public const string IdpChangePasswordPolicyName = "IdpChangePasswordPolicy";
}
