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
public class GetViewGroupsCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public ViewGroupInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ViewGroupInfo[])results[0];
		}
	}

	public GetViewGroupsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
