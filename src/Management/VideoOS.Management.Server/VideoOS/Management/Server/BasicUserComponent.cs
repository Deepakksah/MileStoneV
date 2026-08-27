using System;

namespace VideoOS.Management.Server;

public sealed class BasicUserComponent : GeminiResourceComponent, IBasicUserComponent, IDisposable
{
	public void DeleteBasicUser(string id, string sid, string userName, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_basic_user");
			BindGuidType("@id", id);
			BindSIDType("@sid", sid);
			BindNameType("@user_name", userName.ToUpperInvariant());
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
