using System;
using System.Globalization;
using VideoOS.Installer.Common.Exceptions;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI.ErrorHandling;

public class ErrorHandler
{
	public static FlowManager FlowManager { get; set; }

	public static void ShowError()
	{
		FlowManager.TriggerEvent("ErrorOccured");
	}

	public static void ShowError(string headLine, string subLine, ErrorAction action, string errorText, bool enablePreviousButton)
	{
		ErrorInfo.SetErrorInfo(headLine, subLine, action, errorText, enablePreviousButton);
		FlowManager.TriggerEvent("ErrorOccured");
	}

	public static void ShowError(string headline, ContentLinkText subLineWithLinks, bool enablePreviousButton)
	{
		ErrorInfo.SetErrorInfo(headline, subLineWithLinks, enablePreviousButton);
		FlowManager.TriggerEvent("ErrorOccured");
	}

	public static void ShowError(Exception exception)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		if (exception is UIFrameworkErrorException)
		{
			ShowUIFrameworkErrorException((UIFrameworkErrorException)exception);
		}
		else if (exception is DetailedErrorException || exception.InnerException is DetailedErrorException)
		{
			ShowDetailedErrorException((exception as DetailedErrorException) ?? (exception.InnerException as DetailedErrorException));
		}
		else if (exception is LocalizedException)
		{
			ShowLocalizedException((LocalizedException)exception);
		}
		else
		{
			ShowGeneralException(exception);
		}
	}

	public static void ShowUninstallationError(Exception ex)
	{
		ShowErrorWithDetails(string.Format(Resources.ErrorMessage_GeneralErrorUninstallLabelSubLine, VersionFormat.GetNeutralProductName()), ex.Message, enablePreviousButton: false);
	}

	private static void ShowUIFrameworkErrorException(UIFrameworkErrorException exception)
	{
		ShowErrorWithDetails(Resources.ErrorMessage_GeneralErrorLabelSubLine, exception.Message, exception.EnablePreviousButton);
	}

	private static void ShowDetailedErrorException(DetailedErrorException exception)
	{
		ShowError(exception.ErrorTitle, exception.ErrorBody, exception.EnablePreviousButton);
	}

	private static void ShowLocalizedException(LocalizedException exception)
	{
		ShowErrorWithDetails(Resources.ErrorMessage_GeneralErrorLabelSubLine, exception.DisplayMessage, enablePreviousButton: false);
	}

	private static void ShowGeneralException(Exception exception)
	{
		ShowErrorWithDetails(Resources.ErrorMessage_GeneralErrorLabelSubLine, exception.Message, enablePreviousButton: false);
	}

	private static void ShowErrorWithDetails(string errorLabelSubLine, string errorDetails, bool enablePreviousButton)
	{
		ShowError(Resources.ErrorMessage_GeneralErrorLabelHeadLine, errorLabelSubLine, new ViewLogAction(LogManager.GetLogFilename()), (!string.IsNullOrEmpty(errorDetails)) ? string.Format(CultureInfo.CurrentCulture, Resources.ErrorMessage_GeneralErrorLabelMessage, errorDetails) : string.Empty, enablePreviousButton);
	}
}
