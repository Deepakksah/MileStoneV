using System;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server.RestApi;

public class RestApiSecurityUtil
{
	public static void CheckConnect(IGeminiRequestContext requestContext)
	{
		try
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "CONNECT");
		}
		catch (UnauthorizedAccessException ex)
		{
			throw new UnauthorizedException(ex.Message);
		}
	}
}
