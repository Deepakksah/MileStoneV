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
public class SetCustomSettingDataUserCompletedEventArgs : AsyncCompletedEventArgs, ISetCustomSettingDataUserCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	VideoOS.Platform.SDK.Interface.SetCustomSettingResultType ISetCustomSettingDataUserCompletedEventArgs.Result => Result switch
	{
		SetCustomSettingResultType.ModifiedError => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.ModifiedError, 
		SetCustomSettingResultType.Success => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.Success, 
		SetCustomSettingResultType.UnspecifiedError => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.UnspecifiedError, 
		_ => VideoOS.Platform.SDK.Interface.SetCustomSettingResultType.UnspecifiedError, 
	};

	public SetCustomSettingResultType Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SetCustomSettingResultType)results[0];
		}
	}

	public SetCustomSettingDataUserCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
