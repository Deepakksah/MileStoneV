using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetRetentionTimeLimitCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public DateTime Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DateTime)results[0];
		}
	}

	public GetRetentionTimeLimitCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
