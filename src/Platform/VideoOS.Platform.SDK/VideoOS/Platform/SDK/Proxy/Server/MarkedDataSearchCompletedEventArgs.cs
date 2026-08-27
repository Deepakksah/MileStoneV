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
public class MarkedDataSearchCompletedEventArgs : AsyncCompletedEventArgs, IMarkedDataSearchCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	IMarkedData[] IMarkedDataSearchCompletedEventArgs.Result => Result;

	public MarkedData[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (MarkedData[])results[0];
		}
	}

	public MarkedDataSearchCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
