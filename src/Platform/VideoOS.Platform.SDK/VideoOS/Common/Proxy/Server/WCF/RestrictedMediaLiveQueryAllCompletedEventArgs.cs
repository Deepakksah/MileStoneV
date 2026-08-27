using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RestrictedMediaLiveQueryAllCompletedEventArgs : AsyncCompletedEventArgs, IRestrictedMediaLiveQueryAllCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public RestrictedMediaLive[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RestrictedMediaLive[])results[0];
		}
	}

	IRestrictedMediaLive[] IRestrictedMediaLiveQueryAllCompletedEventArgs.Results => Result;

	public RestrictedMediaLiveQueryAllCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
