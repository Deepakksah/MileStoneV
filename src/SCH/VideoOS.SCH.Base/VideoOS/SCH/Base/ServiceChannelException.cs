using System;
using System.Collections.Generic;

namespace VideoOS.SCH.Base;

public class ServiceChannelException : Exception
{
	public class ErrorItem
	{
		public int ErrorNumber { get; set; }

		public string ErrorDescription { get; set; }

		public ErrorItem(int errorNumber, string errorDescription)
		{
			ErrorNumber = errorNumber;
			ErrorDescription = errorDescription;
		}
	}

	public List<ErrorItem> Errors = new List<ErrorItem>();

	public bool Logged { get; set; }

	public new virtual string Message
	{
		get
		{
			string text = null;
			if (Errors.Count > 0)
			{
				text = "Service Channel error number: " + Errors[0].ErrorNumber;
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
			return base.Message + Environment.NewLine + text;
		}
	}

	public ServiceChannelException()
	{
	}

	public ServiceChannelException(string message)
		: base(message)
	{
	}

	public ServiceChannelException(int errorCode)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ServiceChannelException(int errorCode, string message)
		: base(message)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ServiceChannelException(int errorCode, string message, Exception innerException)
		: base(message, innerException)
	{
		Errors.Add(new ErrorItem(errorCode, null));
	}

	public ServiceChannelException(string message, List<ErrorItem> errors, Exception innerException)
		: base(message, innerException)
	{
		Errors = errors;
	}
}
