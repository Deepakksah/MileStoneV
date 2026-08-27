using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class JPEGGetAtOrAfterCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public JPEGData Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (JPEGData)results[0];
		}
	}

	public JPEGGetAtOrAfterCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
