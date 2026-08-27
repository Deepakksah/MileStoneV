using System;
using System.Globalization;
using System.Text;

namespace VideoOS.Common.Controls;

public static class ExceptionDetailsFormatter
{
	public static string BuildTechnicalDetailsText(Exception exception)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		string value = string.Format(CultureInfo.CurrentCulture, "{0}------------------------------{0}", Environment.NewLine);
		StringBuilder stringBuilder = new StringBuilder();
		int num = 1;
		for (Exception ex = exception; ex != null; ex = ex.InnerException)
		{
			if (num > 1)
			{
				stringBuilder.Append(value);
			}
			if (exception.InnerException != null && num == 2)
			{
				stringBuilder.AppendLine("Additional info:");
			}
			if (num > 1)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine(ex.GetType().FullName);
			stringBuilder.AppendLine();
			if (!string.IsNullOrEmpty(ex.Message))
			{
				stringBuilder.Append(ex.Message);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Program location:");
			stringBuilder.AppendLine(ex.StackTrace);
			num++;
		}
		return stringBuilder.ToString();
	}
}
