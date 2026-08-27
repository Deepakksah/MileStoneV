using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Utilities.Validation;

public static class ValidationExtensions
{
	public static void NullCheck<T>([ValidatedNotNull] this T parameter, string optionalText = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) where T : class
	{
		if (parameter != null)
		{
			return;
		}
		throw new ArgumentNullException(Checks.Format<T>(optionalText, memberName, sourceFilePath, sourceLineNumber));
	}

	public static void NullCheck([ValidatedNotNull] this string parameter, string optionalText = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (parameter != null)
		{
			return;
		}
		throw new ArgumentNullException(Checks.Format<string>(optionalText, memberName, sourceFilePath, sourceLineNumber));
	}
}
