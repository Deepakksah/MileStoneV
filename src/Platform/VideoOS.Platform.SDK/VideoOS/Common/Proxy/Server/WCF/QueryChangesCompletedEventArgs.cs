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
public class QueryChangesCompletedEventArgs : AsyncCompletedEventArgs, IQueryChangesCompletedEventArgs
{
	private object[] results;

	public Change[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Change[])results[0];
		}
	}

	IChange[] IQueryChangesCompletedEventArgs.Result => Result;

	public QueryChangesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
