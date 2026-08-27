using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VideoOS.Utilities.Validation;

public static class Checks
{
	public static void IsNull<T>([ValidatedNotNull] T parameter, string optionalText = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) where T : class
	{
		if (parameter != null)
		{
			return;
		}
		throw new ArgumentNullException(Format<T>(optionalText, memberName, sourceFilePath, sourceLineNumber));
	}

	public static string Format<T>(string optionalText, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		string arg = string.Format(CultureInfo.InvariantCulture, "caller = {0}{3}source file ={1}{3}source line = {2}{3}", memberName, sourceFilePath, sourceLineNumber, Environment.NewLine);
		string fullName = typeof(T).FullName;
		string arg2 = (string.IsNullOrEmpty(optionalText) ? string.Empty : string.Format(CultureInfo.InvariantCulture, "Additional information: {0}{1}", optionalText, Environment.NewLine));
		string arg3 = string.Format(CultureInfo.InvariantCulture, "Argument of type {0} was null{1}{2}", fullName, Environment.NewLine, arg2);
		return string.Format(CultureInfo.InvariantCulture, "{0}{1}", arg, arg3);
	}
}
