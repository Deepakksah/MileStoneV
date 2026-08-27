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
public class GetConfigurationCompletedEventArgs : AsyncCompletedEventArgs, IGetConfigurationCompletedEventArgs
{
	private object[] results;

	IConfigurationInfo IGetConfigurationCompletedEventArgs.Result => Result;

	public ConfigurationInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ConfigurationInfo)results[0];
		}
	}

	public GetConfigurationCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
