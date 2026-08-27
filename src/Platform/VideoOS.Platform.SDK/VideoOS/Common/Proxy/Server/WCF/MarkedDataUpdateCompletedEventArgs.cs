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
public class MarkedDataUpdateCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataUpdateCompletedEventArgs, IAsyncCompletedEventArgs
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

	IMarkedDataResult IMarkedDataUpdateCompletedEventArgs.Result => Result;

	public MarkedDataUpdateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
