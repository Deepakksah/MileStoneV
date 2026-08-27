using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[ExcludeFromCodeCoverage]
public class MarkedDataGetNewReferenceCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataGetNewReferenceCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	IMarkedDataReference IMarkedDataGetNewReferenceCompletedEventArgs.Result => Result;

	public MarkedDataReference Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MarkedDataReference)results[0];
		}
	}

	public MarkedDataGetNewReferenceCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
