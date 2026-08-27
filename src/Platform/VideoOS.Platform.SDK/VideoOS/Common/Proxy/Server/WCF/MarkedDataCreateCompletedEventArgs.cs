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
public class MarkedDataCreateCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataCreateCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public MarkedDataResult Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MarkedDataResult)results[0];
		}
	}

	IMarkedDataResult IMarkedDataCreateCompletedEventArgs.Result => Result;

	public MarkedDataCreateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
