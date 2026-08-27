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
public class GetPresetsCompletedEventArgs : AsyncCompletedEventArgs, IGetPresetsCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public PresetInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (PresetInfo[])results[0];
		}
	}

	IPresetInfo[] IGetPresetsCompletedEventArgs.Result => Result;

	public GetPresetsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
