using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.RecorderWebservice.Shared;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class IsManualRecordingCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public ManualRecordingInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ManualRecordingInfo[])results[0];
		}
	}

	public IsManualRecordingCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
