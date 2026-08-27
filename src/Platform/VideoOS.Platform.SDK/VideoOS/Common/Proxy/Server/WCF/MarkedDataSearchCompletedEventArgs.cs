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
public class MarkedDataSearchCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataSearchCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public MarkedData[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MarkedData[])results[0];
		}
	}

	IMarkedData[] IMarkedDataSearchCompletedEventArgs.Result => Result;

	public MarkedDataSearchCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
