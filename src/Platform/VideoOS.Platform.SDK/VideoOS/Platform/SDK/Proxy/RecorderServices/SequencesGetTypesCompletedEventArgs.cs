using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[GeneratedCode("wsdl", "4.0.30319.33440")]
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
