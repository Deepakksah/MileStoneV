using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DashboardHistoryEnabledManager : ISystemSettingManager
{
	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			try
			{
				if (!bool.TryParse(value, out var _))
				{
					throw new ArgumentException("Dashboard history enabled needs to be a boolean.");
				}
				return value;
			}
			catch (FormatException)
			{
				throw new SystemSettingInvalidValueException("DashboardHistoryEnabled", value);
			}
		}
		return string.Empty;
	}

	public string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public void CheckGetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, "GENERIC_READ", "/");
	}

	public void CheckSetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, "GENERIC_WRITE", "/");
	}
}
