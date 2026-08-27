using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.Status;

[GeneratedCode("wsdl", "2.0.50727.1432")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class StartStatusSessionCompletedEventArgs : AsyncCompletedEventArgs
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

	public StartStatusSessionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
