using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class RestrictedMediaQueryAllCompletedEventArgs : AsyncCompletedEventArgs, IRestrictedMediaQueryAllCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public RestrictedMedia[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RestrictedMedia[])results[0];
		}
	}

	IRestrictedMedia[] IRestrictedMediaQueryAllCompletedEventArgs.Results => Result;

	public RestrictedMediaQueryAllCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
