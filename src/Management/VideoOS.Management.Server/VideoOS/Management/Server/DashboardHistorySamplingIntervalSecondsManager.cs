using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DashboardHistorySamplingIntervalSecondsManager : ISystemSettingManager
{
	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			try
			{
				if (!int.TryParse(value, out var _))
				{
					throw new ArgumentException("Dashboard history sampling interval seconds needs to be a integer.");
				}
				return value;
			}
			catch (FormatException)
			{
				throw new SystemSettingInvalidValueException("DashboardHistorySamplingIntervalSeconds", value);
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
