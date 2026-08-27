using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.RecorderWebservice.Shared;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class OutputGetStateCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public IOState Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (IOState)results[0];
		}
	}

	public OutputGetStateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
