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
public class GetUserInfoCompletedEventArgs : AsyncCompletedEventArgs, IGetUserInfoCompletedEventArgs
{
	private object[] results;

	public UserInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (UserInfo)results[0];
		}
	}

	IUserInfo IGetUserInfoCompletedEventArgs.Result => Result;

	public GetUserInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
