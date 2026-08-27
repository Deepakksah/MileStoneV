using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VideoOS.Platform.SDK.Proxy.Server;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[ExcludeFromCodeCoverage]
public class GetViewGroupDataCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public ViewGroup Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ViewGroup)results[0];
		}
	}

	public GetViewGroupDataCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
