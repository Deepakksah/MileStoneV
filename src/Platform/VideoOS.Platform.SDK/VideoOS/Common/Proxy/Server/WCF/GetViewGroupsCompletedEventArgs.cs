using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
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
