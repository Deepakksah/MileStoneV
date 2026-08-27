using System;

namespace VideoOS.Platform.Util.ImageServer;

public static class UserIdType
{
	public enum UserIDType
	{
		InvalidIdentity,
		UnknownIdentityType,
		WindowsUser,
		WindowsGroup,
		Role,
		BasicUser,
		OAuthUser
	}

	public static class UserIDTypeConstants
	{
		public static readonly Guid InvalidIdentityGuid = new Guid("736528d6-d3ec-4e93-8bbf-e7b3f707289f");

		public static readonly Guid UnknownIdentityGuid = new Guid("994e3f49-d527-40b9-989f-ffbc9a37ed93");

		public static readonly Guid WindowsUserGuid = new Guid("a9f79659-3434-4bd3-b462-b0dde1a3d637");

		public static readonly Guid WindowsGroupGuid = new Guid("2e651aef-fc99-4bbc-b4c6-12230440edae");

		public static readonly Guid RoleGuid = new Guid("3884076c-b993-406d-b5f1-4bd795210fc3");

		public static readonly Guid BasicUserGuid = new Guid("8729b284-c3ee-40f7-b452-1ab9930867cb");
	}

	public static class InternalSID
	{
		public static string CreateInternalSID(string userName)
		{
			return "B-" + userName;
		}

		public static string GetUserNameFromInternalSID(string sid)
		{
			if (sid != null && sid.Length > 2)
			{
				return sid.Substring(2);
			}
			return "";
		}
	}
}
