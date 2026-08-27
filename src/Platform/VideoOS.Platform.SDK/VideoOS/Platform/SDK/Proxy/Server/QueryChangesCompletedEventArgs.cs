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
public class QueryChangesCompletedEventArgs : AsyncCompletedEventArgs, IQueryChangesCompletedEventArgs
{
	private object[] results;

	IChange[] IQueryChangesCompletedEventArgs.Result => Result;

	public Change[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Change[])results[0];
		}
	}

	public QueryChangesCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
