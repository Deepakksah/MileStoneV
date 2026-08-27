using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public static class GeminiExceptionFormatter
{
	public static string FormatException(Exception ex, bool includeDetails)
	{
		if (ex == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(FormatExceptionInternal(ex, includeDetails));
		if (includeDetails)
		{
			for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(Resources.InnerException);
				stringBuilder.Append(FormatExceptionInternal(innerException, includeDetails: true));
			}
		}
		return stringBuilder.ToString();
	}

	private static string FormatExceptionInternal(Exception ex, bool includeDetails)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Format(CultureInfo.InvariantCulture, Resources.ExceptionHeader, ex.Message, ex.GetType().Name));
		if (includeDetails)
		{
			if (ex is DatabaseException ex2)
			{
				string value = string.Format(CultureInfo.InvariantCulture, Resources.SqlExceptionDetails, ex2.Class, ex2.Number, ex2.Procedure, ex2.LineNumber, ex2.Server, ex2.State, FormatSqlErrors(ex2));
				stringBuilder.AppendLine(value);
			}
			else
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine(FormatData(ex));
			stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, Resources.ExceptionStackTrace, ex.StackTrace));
		}
		return stringBuilder.ToString();
	}

	private static string FormatSqlErrors(DatabaseException ex)
	{
		if (ex.Errors.Count() == 1)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < ex.Errors.Count(); i++)
		{
			string value = string.Format(CultureInfo.InvariantCulture, Resources.SqlErrorDetails, i, ex.Errors[i].ToString(), ex.Errors[i].Class, ex.Errors[i].Number, ex.Errors[i].Server, ex.Errors[i].Source, ex.Errors[i].State, ex.Errors[i].Procedure, ex.Errors[i].LineNumber);
			stringBuilder.AppendLine(value);
		}
		return stringBuilder.ToString();
	}

	private static string FormatData(Exception ex)
	{
		if (ex.Data == null || ex.Data.Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DictionaryEntry datum in ex.Data)
		{
			string value = string.Format(CultureInfo.InvariantCulture, Resources.ExceptionDataDictionary, datum.Key, datum.Value);
			stringBuilder.AppendLine(value);
		}
		return string.Format(CultureInfo.InvariantCulture, Resources.ExceptionDataDetails, stringBuilder.ToString());
	}
}
