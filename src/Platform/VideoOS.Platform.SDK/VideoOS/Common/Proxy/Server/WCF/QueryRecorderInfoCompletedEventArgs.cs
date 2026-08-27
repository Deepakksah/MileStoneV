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
public class QueryRecorderInfoCompletedEventArgs : AsyncCompletedEventArgs, IQueryRecorderInfoCompletedEventArgs
{
	private object[] results;

	public RecorderInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RecorderInfo)results[0];
		}
	}

	IRecorderInfo IQueryRecorderInfoCompletedEventArgs.Result => Result;

	public QueryRecorderInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
