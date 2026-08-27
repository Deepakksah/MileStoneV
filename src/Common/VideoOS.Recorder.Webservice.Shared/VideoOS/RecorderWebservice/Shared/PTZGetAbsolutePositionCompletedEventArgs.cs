using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.RecorderWebservice.Shared;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class PTZGetAbsolutePositionCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public PTZArgs Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (PTZArgs)results[0];
		}
	}

	public PTZGetAbsolutePositionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
