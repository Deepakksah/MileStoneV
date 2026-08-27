using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class SetViewGroupDataCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public SetViewGroupDataResultType Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SetViewGroupDataResultType)results[0];
		}
	}

	public SetViewGroupDataCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
