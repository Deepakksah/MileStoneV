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
public class GetCustomSettingDataUserCompletedEventArgs : AsyncCompletedEventArgs, IGetCustomSettingDataUserCompletedEventArgs
{
	private object[] results;

	public CustomSettingInternal Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (CustomSettingInternal)results[0];
		}
	}

	ICustomSetting IGetCustomSettingDataUserCompletedEventArgs.Result => Result;

	public GetCustomSettingDataUserCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
