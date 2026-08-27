using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class MarkedDataGetNewReferenceCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataGetNewReferenceCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public MarkedDataReference Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MarkedDataReference)results[0];
		}
	}

	IMarkedDataReference IMarkedDataGetNewReferenceCompletedEventArgs.Result => Result;

	public MarkedDataGetNewReferenceCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
