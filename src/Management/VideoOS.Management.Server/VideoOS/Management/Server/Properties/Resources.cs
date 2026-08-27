using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.Server.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Management.Server.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ActiveDirectoryAuthenticationException => ResourceManager.GetString("ActiveDirectoryAuthenticationException", resourceCulture);

	public static string ActiveDirectoryNoDomainException => ResourceManager.GetString("ActiveDirectoryNoDomainException", resourceCulture);

	public static string AssertionFailureHeader => ResourceManager.GetString("AssertionFailureHeader", resourceCulture);

	public static string AuthenticationFailure => ResourceManager.GetString("AuthenticationFailure", resourceCulture);

	public static string AuthenticationSuccess => ResourceManager.GetString("AuthenticationSuccess", resourceCulture);

	public static string Category => ResourceManager.GetString("Category", resourceCulture);

	public static string ContinuePendingStatus => ResourceManager.GetString("ContinuePendingStatus", resourceCulture);

	public static string Entering => ResourceManager.GetString("Entering", resourceCulture);

	public static string EventSourceMissingException => ResourceManager.GetString("EventSourceMissingException", resourceCulture);

	public static string Exception => ResourceManager.GetString("Exception", resourceCulture);

	public static string ExceptionDataDetails => ResourceManager.GetString("ExceptionDataDetails", resourceCulture);

	public static string ExceptionDataDictionary => ResourceManager.GetString("ExceptionDataDictionary", resourceCulture);

	public static string ExceptionHeader => ResourceManager.GetString("ExceptionHeader", resourceCulture);

	public static string ExceptionStackTrace => ResourceManager.GetString("ExceptionStackTrace", resourceCulture);

	public static string FailedReadingTracingConfiguration => ResourceManager.GetString("FailedReadingTracingConfiguration", resourceCulture);

	public static string HostingServerInvalidEntityException => ResourceManager.GetString("HostingServerInvalidEntityException", resourceCulture);

	public static string InformationalEventFormat => ResourceManager.GetString("InformationalEventFormat", resourceCulture);

	public static string InformationalEventFormatNoHost => ResourceManager.GetString("InformationalEventFormatNoHost", resourceCulture);

	public static string InnerException => ResourceManager.GetString("InnerException", resourceCulture);

	public static string Leaving => ResourceManager.GetString("Leaving", resourceCulture);

	public static string LogOutputString => ResourceManager.GetString("LogOutputString", resourceCulture);

	public static string MessageBrief => ResourceManager.GetString("MessageBrief", resourceCulture);

	public static string Method => ResourceManager.GetString("Method", resourceCulture);

	public static string MIP => ResourceManager.GetString("MIP", resourceCulture);

	public static string NotAvailable => ResourceManager.GetString("NotAvailable", resourceCulture);

	public static string PausedStatus => ResourceManager.GetString("PausedStatus", resourceCulture);

	public static string PauseOperation => ResourceManager.GetString("PauseOperation", resourceCulture);

	public static string PausePendingStatus => ResourceManager.GetString("PausePendingStatus", resourceCulture);

	public static string ResumeOperation => ResourceManager.GetString("ResumeOperation", resourceCulture);

	public static string RunningStatus => ResourceManager.GetString("RunningStatus", resourceCulture);

	public static string SqlErrorDetails => ResourceManager.GetString("SqlErrorDetails", resourceCulture);

	public static string SqlExceptionDetails => ResourceManager.GetString("SqlExceptionDetails", resourceCulture);

	public static string StackFrameHeader => ResourceManager.GetString("StackFrameHeader", resourceCulture);

	public static string StackFrameLineFormat => ResourceManager.GetString("StackFrameLineFormat", resourceCulture);

	public static string StartOperation => ResourceManager.GetString("StartOperation", resourceCulture);

	public static string StartPendingStatus => ResourceManager.GetString("StartPendingStatus", resourceCulture);

	public static string StopOperation => ResourceManager.GetString("StopOperation", resourceCulture);

	public static string StoppedStatus => ResourceManager.GetString("StoppedStatus", resourceCulture);

	public static string StopPendingStatus => ResourceManager.GetString("StopPendingStatus", resourceCulture);

	public static string VmoInvalidSettingException => ResourceManager.GetString("VmoInvalidSettingException", resourceCulture);

	public static string WebRequestStart => ResourceManager.GetString("WebRequestStart", resourceCulture);

	public static string WebRequestStop => ResourceManager.GetString("WebRequestStop", resourceCulture);

	public Resources()
	{
	}
}
