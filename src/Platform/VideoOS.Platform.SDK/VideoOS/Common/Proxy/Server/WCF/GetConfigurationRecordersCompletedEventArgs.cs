using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class GetConfigurationRecordersCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public RecorderInfo[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (RecorderInfo[])results[0];
		}
	}

	public GetConfigurationRecordersCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
