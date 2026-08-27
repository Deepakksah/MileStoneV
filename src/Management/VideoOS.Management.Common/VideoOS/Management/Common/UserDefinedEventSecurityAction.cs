namespace VideoOS.Management.Common;

public static class UserDefinedEventSecurityAction
{
	public const string Read = "GENERIC_READ";

	public const string Edit = "GENERIC_WRITE";

	public const string Delete = "DELETE";

	public const string Trigger = "TRIGGER";

	public const string Create = "CREATE_USER_DEFINED_EVENT";

	public const string AdministrateSecurity = "ADMIN_SECURITY";
}
