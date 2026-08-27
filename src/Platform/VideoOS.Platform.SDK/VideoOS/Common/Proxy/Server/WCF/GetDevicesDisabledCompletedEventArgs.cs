using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class GetDevicesDisabledCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public DeviceDisabledInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DeviceDisabledInfo[])results[0];
		}
	}

	public GetDevicesDisabledCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
