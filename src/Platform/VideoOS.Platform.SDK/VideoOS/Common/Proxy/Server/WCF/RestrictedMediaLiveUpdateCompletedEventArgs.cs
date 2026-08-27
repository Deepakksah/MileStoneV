using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RestrictedMediaLiveUpdateCompletedEventArgs : AsyncCompletedEventArgs, IRestrictedMediaLiveUpdateCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public RestrictedMediaLiveResult Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RestrictedMediaLiveResult)results[0];
		}
	}

	IRestrictedMediaLiveResult IRestrictedMediaLiveUpdateCompletedEventArgs.Result => Result;

	public RestrictedMediaLiveUpdateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
