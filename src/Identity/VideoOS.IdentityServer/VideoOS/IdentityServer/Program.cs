using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace VideoOS.IdentityServer;

public static class Program
{
	public static void Main(string[] args)
	{
		Logger currentClassLogger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
		try
		{
			currentClassLogger.Info("Application starting...");
			CreateWebHostBuilder(args).Build().Run();
		}
		catch (Exception value)
		{
			currentClassLogger.Error(value);
			Console.WriteLine(value);
			Environment.Exit(-1);
			throw;
		}
		finally
		{
			currentClassLogger.Info("Application shutdown...");
			LogManager.Shutdown();
		}
	}

	public static IWebHostBuilder CreateWebHostBuilder(string[] args)
	{
		IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(args).UseIISIntegration().UseContentRoot(Directory.GetCurrentDirectory())
			.UseStartup<Startup>()
			.ConfigureLogging(delegate(ILoggingBuilder logging)
			{
				logging.ClearProviders();
			})
			.CaptureStartupErrors(captureStartupErrors: false)
			.UseNLog(new NLogAspNetCoreOptions
			{
				RemoveLoggerFactoryFilter = false
			});
		if (string.IsNullOrEmpty(webHostBuilder.GetSetting("IIS_HTTPAUTH")))
		{
			webHostBuilder = webHostBuilder.UseHttpSys(delegate(HttpSysOptions options)
			{
				options.Authentication.Schemes = AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
			});
		}
		return webHostBuilder;
	}
}
