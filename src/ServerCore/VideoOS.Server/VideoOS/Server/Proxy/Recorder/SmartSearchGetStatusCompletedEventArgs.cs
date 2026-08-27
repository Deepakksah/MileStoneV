using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SmartSearchGetStatusCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public SmartSearchStatus Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SmartSearchStatus)results[0];
		}
	}

	public SmartSearchGetStatusCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
