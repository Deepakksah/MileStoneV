using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class StopManualRecordingCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public ManualRecordingResult[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ManualRecordingResult[])results[0];
		}
	}

	public StopManualRecordingCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
