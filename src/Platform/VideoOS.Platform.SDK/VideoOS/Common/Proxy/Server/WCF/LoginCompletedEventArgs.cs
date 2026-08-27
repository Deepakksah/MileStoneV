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
public class LoginCompletedEventArgs : AsyncCompletedEventArgs, ILoginCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public LoginInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (LoginInfo)results[0];
		}
	}

	ILoginInfo ILoginCompletedEventArgs.Result => Result;

	public LoginCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
