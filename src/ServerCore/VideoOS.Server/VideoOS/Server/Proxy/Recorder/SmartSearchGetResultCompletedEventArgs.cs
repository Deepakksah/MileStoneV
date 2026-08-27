using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SmartSearchGetResultCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public SmartSearchResult Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SmartSearchResult)results[0];
		}
	}

	public SmartSearchGetResultCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
