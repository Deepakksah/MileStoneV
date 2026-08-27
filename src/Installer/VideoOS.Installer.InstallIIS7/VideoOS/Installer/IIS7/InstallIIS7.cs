using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Dism;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.IIS7;

public class InstallIIS7
{
	public ErrorType ErrorType { get; private set; } = ErrorType.HasNotStarted;

	public bool Install()
	{
		if (NativeMethods.IsWindows7or8Embedded())
		{
			LogManager.Write("Windows 7 or 8 Embedded detected - skipping IIS installation");
			ErrorType = ErrorType.Success;
			return true;
		}
		DismSession dismSession = null;
		FeatureInstallResult featureInstallResult = null;
		try
		{
			dismSession = new DismSession((ILogger)(object)new LogWrapper());
			dismSession.Initialize(null);
			featureInstallResult = dismSession.EnsureFeaturesAreInstalled(Feature.IISCommonHttpFeature, Feature.IISWebserverRole, Feature.IISWebserver, Feature.IISStaticContent, Feature.IISDefaultDocument, Feature.IISDirectoryBrowsing, Feature.IISHttpErrors, Feature.IISApplicationDevelopment, Feature.IISHealthAndDiagnostics, Feature.IISHttpLogging, Feature.IISRequestMonitor, Feature.IISSecurity, Feature.IISBasicAuthentication, Feature.IISWindowsAuthentication, Feature.IISRequestFiltering, Feature.IISPerformance, Feature.IISHttpCompressionStatic, Feature.IISWebserverManagementTools, Feature.IISManagementConsole, Feature.WasWindowsActivationService, Feature.WasProcessModel, Feature.WasConfigurationApi, Feature.IIS_ISApiFilter, Feature.IIS_ISApiExtensions, Feature.IISAspNet45, Feature.IISNetFxExtensibility45, Feature.WcfHttpActivation45, Feature.IISApplicationInit);
			ErrorType? errorType = null;
			switch (featureInstallResult.Status)
			{
			case InstallStatus.Installed:
				errorType = ErrorType.Success;
				break;
			case InstallStatus.NeedsReboot:
				errorType = ErrorType.NeedReboot;
				break;
			case InstallStatus.FeatureNotAvailable:
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "The feature {0} is not available", featureInstallResult.FeatureName));
				break;
			case InstallStatus.InstallationFailure:
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "The Installation of the feature {0} failed with error message \"{1}\"", featureInstallResult.FeatureName, featureInstallResult.ErrorMessage));
				break;
			default:
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Unexpeced result of the installation : {0}", featureInstallResult.FeatureName));
				break;
			}
			ErrorType = (ErrorType)(((int?)errorType) ?? (NativeMethods.IsWorkstation() ? 3 : 2));
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage());
		}
		finally
		{
			try
			{
				dismSession?.Shutdown();
			}
			catch (Exception ex2)
			{
				LogManager.Write(ex2.TypeAndMessage());
			}
		}
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Install status: {0}", featureInstallResult?.Status ?? InstallStatus.InstallationFailure));
		if (ErrorType == ErrorType.Success)
		{
			GlobalData.SetValue("IISManagementConsoleInstalled", true.ToString(), "IISSetupPageActions()");
		}
		return ErrorType == ErrorType.Success;
	}
}
