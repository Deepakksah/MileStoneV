using System;
using System.Net;
using System.Security.Principal;
using System.Text;
using VideoOS.UI.Common.WPF.Resources;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public static class ExceptionUtils
{
	public static string ExceptionToDetailsText(Exception exception)
	{
		return string.Concat(string.Concat(string.Empty + ExceptionToString(exception), Environment.NewLine), BuildSystemInfo());
	}

	private static string ExceptionToString(Exception e, bool isInnerException = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (e.InnerException != null)
		{
			stringBuilder.Append(ExceptionToString(e.InnerException, isInnerException: true));
			stringBuilder.Append(Environment.NewLine);
		}
		if (isInnerException)
		{
			stringBuilder.Append(Strings.MessageBoxInnerException);
		}
		else
		{
			stringBuilder.Append(Strings.MessageBoxOuterException);
		}
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxExceptionType);
		stringBuilder.Append(e.GetType().FullName);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxExceptionMessage);
		stringBuilder.Append(e.Message);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxExceptionSource);
		stringBuilder.Append((e.Source == null) ? e.Message : e.Source);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxExceptionTargetSite);
		stringBuilder.Append((e.TargetSite == null) ? e.Message : e.TargetSite.Name);
		stringBuilder.Append(Environment.NewLine);
		if (e.StackTrace != null)
		{
			string value = e.StackTrace.ToString();
			stringBuilder.Append(value);
			stringBuilder.Append(Environment.NewLine);
		}
		return stringBuilder.ToString();
	}

	private static string BuildSystemInfo()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Strings.MessageBoxSystemInfo);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxDateTime);
		stringBuilder.Append("\t");
		stringBuilder.Append(DateTime.Now);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxMachineName);
		stringBuilder.Append("\t");
		stringBuilder.Append(Environment.MachineName);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxProcessorCount);
		stringBuilder.Append("\t");
		stringBuilder.Append(Environment.ProcessorCount);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxOSVersion);
		stringBuilder.Append("\t");
		stringBuilder.Append(Environment.OSVersion.ToString());
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxIs64BitOperativSystem);
		stringBuilder.Append("\t");
		stringBuilder.Append(Environment.Is64BitOperatingSystem);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxIs64BitProcess);
		stringBuilder.Append("\t");
		stringBuilder.Append(Environment.Is64BitProcess);
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxIPAddress);
		stringBuilder.Append("\t");
		stringBuilder.Append(GetHostInfo());
		stringBuilder.Append(Environment.NewLine);
		stringBuilder.Append(Strings.MessageBoxCurrentUser);
		stringBuilder.Append("\t");
		stringBuilder.Append(GetUserIdentity());
		stringBuilder.Append(Environment.NewLine);
		return stringBuilder.ToString();
	}

	private static string GetHostInfo()
	{
		try
		{
			string text = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
			string hostName = Dns.GetHostEntry(Dns.GetHostName()).HostName;
			return text + " (" + hostName + ")";
		}
		catch
		{
			return Strings.MessageBoxUnableToGetIP;
		}
	}

	private static string GetUserIdentity()
	{
		string text = WindowsIdentity.GetCurrent().Name;
		if (string.IsNullOrEmpty(text))
		{
			text = Environment.UserDomainName + "\\" + Environment.UserName;
		}
		return text;
	}
}
