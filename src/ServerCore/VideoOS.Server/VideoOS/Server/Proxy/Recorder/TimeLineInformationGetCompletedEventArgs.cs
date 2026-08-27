using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class TimeLineInformationGetCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public TimeLineInformationData[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (TimeLineInformationData[])results[0];
		}
	}

	public TimeLineInformationGetCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
