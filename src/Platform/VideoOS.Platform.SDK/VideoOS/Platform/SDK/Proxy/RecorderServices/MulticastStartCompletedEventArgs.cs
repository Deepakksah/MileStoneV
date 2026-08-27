using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class MulticastStartCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public MulticastInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MulticastInfo[])results[0];
		}
	}

	public MulticastStartCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
