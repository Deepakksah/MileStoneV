using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Server.Proxy.Recorder;

[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SequencesGetTypesCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public SequenceType[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SequenceType[])results[0];
		}
	}

	public SequencesGetTypesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
