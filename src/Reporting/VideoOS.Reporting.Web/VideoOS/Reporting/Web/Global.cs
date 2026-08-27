using System;
using System.Globalization;
using System.Web;
using Syncfusion.Licensing;
using VideoOS.Reporting.Web.Plugin.Base;

namespace VideoOS.Reporting.Web;

public class Global : HttpApplication
{
	protected void Application_Start(object sender, EventArgs e)
	{
		SyncfusionLicenseProvider.RegisterLicense("NRAiBiAaIQQuGjN/V0J+XU9Ac1RDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3pTd0RhWX1beHdURWZUUQ==");
	}

	protected void Application_Error(object sender, EventArgs e)
	{
		HttpApplication httpApplication = sender as HttpApplication;
		Exception lastError = base.Server.GetLastError();
		try
		{
			ServiceLogger.Instance?.LogToServerLog(lastError, string.Format(CultureInfo.InvariantCulture, "Unhandled exception thrown at {0} in {1}", httpApplication.Context.Request.RawUrl, httpApplication?.Context?.CurrentHandler?.GetType().FullName), LogType.Error);
		}
		catch
		{
		}
	}

	protected void Application_End(object sender, EventArgs e)
	{
	}
}
