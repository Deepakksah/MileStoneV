using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RestrictedMediaDeleteCompletedEventArgs : AsyncCompletedEventArgs, IRestrictedMediaDeleteCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public RestrictedMediaResult Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RestrictedMediaResult)results[0];
		}
	}

	IRestrictedMediaResult IRestrictedMediaDeleteCompletedEventArgs.Result => Result;

	public RestrictedMediaDeleteCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
