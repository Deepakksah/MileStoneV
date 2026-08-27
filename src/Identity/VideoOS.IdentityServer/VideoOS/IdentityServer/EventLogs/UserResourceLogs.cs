namespace VideoOS.IdentityServer.EventLogs;

public static class UserResourceLogs
{
	public const string UserCreated = "UserCreated";

	public const string UserDeleted = "UserDeleted";

	public const string UserNeedPasswordChange = "UserNeedPasswordChange";

	public const string UserNeedPasswordChangeCleared = "UserNeedPasswordChangeCleared";

	public const string UserChangePasswordSuccess = "UserChangePasswordSuccess";

	public const string UserEnabled = "UserEnabled";

	public const string UserDisabled = "UserDisabled";

	public const string UserLockedOut = "UserLockedOut";

	public const string UserLockedOutCleared = "UserLockedOutCleared";

	public const string UserDescriptionChanged = "UserDescriptionChanged";

	public const string UserCanChangePassword = "UserCanChangePassword";

	public const string UserCannotChangePassword = "UserCannotChangePassword";

	public const string UserClaimsUpdated = "UserClaimsUpdated";
}
