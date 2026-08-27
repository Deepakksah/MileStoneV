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
public class GetSmartClientVersionCompletedEventArgs : AsyncCompletedEventArgs, IGetSmartClientVersionCompletedEventArgs
{
	private object[] results;

	public SmartClientVersion Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SmartClientVersion)results[0];
		}
	}

	ISmartClientVersion IGetSmartClientVersionCompletedEventArgs.Result => Result;

	public GetSmartClientVersionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
