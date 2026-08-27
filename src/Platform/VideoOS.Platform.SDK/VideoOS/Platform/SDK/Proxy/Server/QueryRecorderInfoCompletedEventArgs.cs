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
public class QueryRecorderInfoCompletedEventArgs : AsyncCompletedEventArgs, IQueryRecorderInfoCompletedEventArgs
{
	private object[] results;

	IRecorderInfo IQueryRecorderInfoCompletedEventArgs.Result => Result;

	public RecorderInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RecorderInfo)results[0];
		}
	}

	public QueryRecorderInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
