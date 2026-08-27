using System;
using System.Web.Services.Protocols;

namespace VideoOS.Diagnostics.Logging.Loggers;

[AttributeUsage(AttributeTargets.Method)]
public class SoapTraceExtensionAttribute : SoapExtensionAttribute
{
	private string filename = "c:\\log.txt";

	private int priority;

	public override Type ExtensionType => typeof(SoapTraceExtension);

	public override int Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public string Filename
	{
		get
		{
			return filename;
		}
		set
		{
			filename = value;
		}
	}
}
