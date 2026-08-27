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
public class AlertsGetAroundWithSpanCompletedEventArgs : AsyncCompletedEventArgs, IAlertsGetAroundWithSpanCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	IAlertOccurrence[] IAlertsGetAroundWithSpanCompletedEventArgs.Result => Result;

	public AlertOccurrence[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (AlertOccurrence[])results[0];
		}
	}

	public AlertsGetAroundWithSpanCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
