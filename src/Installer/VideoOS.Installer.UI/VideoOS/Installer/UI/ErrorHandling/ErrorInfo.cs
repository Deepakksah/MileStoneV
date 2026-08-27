using System.Collections.Generic;
using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.Installer.UI.ErrorHandling;

public static class ErrorInfo
{
	public static string HeadLine { get; set; }

	public static string SubLine { get; set; }

	public static ContentLinkText SubLineLinkText { get; set; }

	public static ErrorAction Action { get; set; }

	public static string ErrorText { get; set; }

	public static bool EnablePreviousButton { get; set; }

	public static ErrorType Type { get; set; }

	public static Dictionary<string, string> PrerequisitesList { get; set; }

	public static void SetErrorInfo(string headLine, string subLine, ErrorAction action, string errorText, bool enablePreviousButton)
	{
		HeadLine = headLine;
		SubLine = subLine;
		SubLineLinkText = null;
		Action = action;
		ErrorText = errorText;
		EnablePreviousButton = enablePreviousButton;
		Type = ErrorType.General;
	}

	public static void SetErrorInfo(string headline, ContentLinkText subLineWithLinks, bool enablePreviousButton)
	{
		SetErrorInfo(headline, subLineWithLinks?.Text, null, null, enablePreviousButton);
		SubLineLinkText = subLineWithLinks;
	}

	public static void SetMissingPrereqErrorInfo(string headLine, string subLine, ErrorAction action, Dictionary<string, string> prerequisitesList, bool enablePreviousButton)
	{
		HeadLine = headLine;
		SubLine = subLine;
		Action = action;
		PrerequisitesList = prerequisitesList;
		EnablePreviousButton = enablePreviousButton;
		Type = ErrorType.MissingPrerequisites;
	}
}
