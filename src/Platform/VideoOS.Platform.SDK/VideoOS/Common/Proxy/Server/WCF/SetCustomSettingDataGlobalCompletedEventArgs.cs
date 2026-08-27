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
public class SetCustomSettingDataGlobalCompletedEventArgs : AsyncCompletedEventArgs, ISetCustomSettingDataGlobalCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public SetCustomSettingResultType Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SetCustomSettingResultType)results[0];
		}
	}

	VideoOS.Platform.SDK.Interface.SetCustomSettingResultType ISetCustomSettingDataGlobalCompletedEventArgs.Result => Result switch
	{
		SetCustomSettingResultType.ModifiedError => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.ModifiedError, 
		SetCustomSettingResultType.Success => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.Success, 
		SetCustomSettingResultType.UnspecifiedError => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.UnspecifiedError, 
		_ => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.UnspecifiedError, 
	};

	public SetCustomSettingDataGlobalCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
