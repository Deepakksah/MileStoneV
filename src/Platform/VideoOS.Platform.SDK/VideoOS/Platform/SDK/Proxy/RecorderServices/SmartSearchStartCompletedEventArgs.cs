using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SmartSearchStartCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public Guid Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Guid)results[0];
		}
	}

	public SmartSearchStartCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
