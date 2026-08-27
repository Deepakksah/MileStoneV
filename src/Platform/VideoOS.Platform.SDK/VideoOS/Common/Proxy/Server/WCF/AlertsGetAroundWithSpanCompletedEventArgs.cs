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
public class AlertsGetAroundWithSpanCompletedEventArgs : AsyncCompletedEventArgs, IAlertsGetAroundWithSpanCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public AlertOccurrence[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (AlertOccurrence[])results[0];
		}
	}

	IAlertOccurrence[] IAlertsGetAroundWithSpanCompletedEventArgs.Result => Result;

	public AlertsGetAroundWithSpanCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
