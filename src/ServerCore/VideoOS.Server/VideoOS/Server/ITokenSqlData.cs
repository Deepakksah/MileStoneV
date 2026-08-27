using System;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Server;

public interface ITokenSqlData
{
	SecurityToken AddToken(string sid, TimeSpan timeout);

	void DeleteToken(string token);

	void ClearExpiredTokens();

	bool GetAllTokens(out SecurityToken[] result);

	SecurityToken GetSecurityToken(string token);
}
