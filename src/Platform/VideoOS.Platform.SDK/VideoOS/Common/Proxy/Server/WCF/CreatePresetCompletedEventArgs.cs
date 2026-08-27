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
public class CreatePresetCompletedEventArgs : AsyncCompletedEventArgs, ICreatePresetCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public Guid Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Guid)results[0];
		}
	}

	public CreatePresetCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
