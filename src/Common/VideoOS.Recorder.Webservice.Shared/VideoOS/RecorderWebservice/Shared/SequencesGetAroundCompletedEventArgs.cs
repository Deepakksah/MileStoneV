using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.RecorderWebservice.Shared;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class SequencesGetAroundCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public SequenceEntry[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SequenceEntry[])results[0];
		}
	}

	public SequencesGetAroundCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
