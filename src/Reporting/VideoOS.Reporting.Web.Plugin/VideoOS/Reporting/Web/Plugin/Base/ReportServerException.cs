using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Reporting.Web.Plugin.Base;

[Serializable]
public class ReportServerException : Exception
{
	public List<ErrorItem> Errors = new List<ErrorItem>();

	public bool Logged { get; set; }

	public override string Message
	{
		get
		{
			string text = null;
			if (Errors.Count > 0)
			{
				text = "Report Server error number: " + Errors[0].ErrorNumber.ToString(CultureInfo.InvariantCulture);
				if (!string.IsNullOrEmpty(Errors[0].ErrorDescription))
				{
					text = text + " [" + Errors[0].ErrorDescription + "]";
				}
			}
			if (string.IsNullOrEmpty(base.Message))
			{
				return text;
			}
			if (text == null)
			{
				return base.Message;
			}
			return base.Message + "; " + text;
		}
	}

	public ReportServerException()
	{
	}

	public ReportServerException(string message)
		: base(message)
	{
	}

	public ReportServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ReportServerException(int errorCode)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ReportServerException(int errorCode, string message)
		: base(message)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ReportServerException(int errorCode, string message, Exception innerException)
		: base(message, innerException)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ReportServerException(string message, List<ErrorItem> errors, Exception innerException)
		: base(message, innerException)
	{
		Errors = errors;
	}
}
