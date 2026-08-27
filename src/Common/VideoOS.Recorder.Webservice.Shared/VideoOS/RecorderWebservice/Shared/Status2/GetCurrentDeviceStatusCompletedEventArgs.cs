using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetCurrentDeviceStatusCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Status Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Status)results[0];
		}
	}

	public GetCurrentDeviceStatusCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
