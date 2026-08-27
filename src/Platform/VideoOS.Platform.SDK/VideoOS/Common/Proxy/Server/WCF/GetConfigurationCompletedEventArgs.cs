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
public class GetConfigurationCompletedEventArgs : AsyncCompletedEventArgs, IGetConfigurationCompletedEventArgs
{
	private object[] results;

	public ConfigurationInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ConfigurationInfo)results[0];
		}
	}

	IConfigurationInfo IGetConfigurationCompletedEventArgs.Result => Result;

	public GetConfigurationCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
