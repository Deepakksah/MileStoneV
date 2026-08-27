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
public class GetSmartClientVersionCompletedEventArgs : AsyncCompletedEventArgs, IGetSmartClientVersionCompletedEventArgs
{
	private object[] results;

	ISmartClientVersion IGetSmartClientVersionCompletedEventArgs.Result => Result;

	public SmartClientVersion Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SmartClientVersion)results[0];
		}
	}

	public GetSmartClientVersionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
