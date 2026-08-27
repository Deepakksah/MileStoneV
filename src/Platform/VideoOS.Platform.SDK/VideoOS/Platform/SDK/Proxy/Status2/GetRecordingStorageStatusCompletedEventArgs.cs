using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class GetRecordingStorageStatusCompletedEventArgs : AsyncCompletedEventArgs
{
	private object[] results;

	public StorageStatus[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (StorageStatus[])results[0];
		}
	}

	public GetRecordingStorageStatusCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
