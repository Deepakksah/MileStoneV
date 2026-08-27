using System.Data;

namespace VideoOS.Management.Server;

public class SecurityResourceComponent : GeminiResourceComponent
{
	protected SecurityResourceComponent()
	{
	}

	protected SecurityResourceComponent(GeminiRequestContext requestContext)
		: base(requestContext)
	{
	}

	protected new void BindObjectId(string parameterName, string parameterValue)
	{
		BindObjectId(parameterName, parameterValue, allowNull: false);
	}

	protected new void BindObjectId(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 1024, allowNull, SqlDbType.NVarChar);
	}

	protected void BindObjectClassId(string parameterName, string parameterValue)
	{
		BindObjectClassId(parameterName, parameterValue, allowNull: false);
	}

	protected void BindObjectClassId(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 1024, allowNull, SqlDbType.VarChar);
	}

	protected void BindActionId(string parameterName, string parameterValue)
	{
		BindActionId(parameterName, parameterValue, allowNull: false);
	}

	protected void BindActionId(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 128, allowNull, SqlDbType.VarChar);
	}

	protected void BindSid(string parameterName, string parameterValue)
	{
		BindSid(parameterName, parameterValue, allowNull: false);
	}

	protected void BindSid(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 256, allowNull, SqlDbType.VarChar);
	}
}
