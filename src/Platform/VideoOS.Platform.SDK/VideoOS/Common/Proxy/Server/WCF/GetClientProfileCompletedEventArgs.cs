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
public class GetClientProfileCompletedEventArgs : AsyncCompletedEventArgs, IGetClientProfileCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public ClientProfile Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ClientProfile)results[0];
		}
	}

	IClientProfile IGetClientProfileCompletedEventArgs.Result => Result;

	public GetClientProfileCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
