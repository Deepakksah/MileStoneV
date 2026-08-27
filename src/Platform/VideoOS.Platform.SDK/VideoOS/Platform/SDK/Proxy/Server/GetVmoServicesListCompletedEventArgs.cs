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
public class GetVmoServicesListCompletedEventArgs : AsyncCompletedEventArgs, IGetVmoServicesListCompletedEventArgs
{
	private object[] results;

	IDictionaryEntry[] IGetVmoServicesListCompletedEventArgs.Result => Result;

	public DictionaryEntry[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (DictionaryEntry[])results[0];
		}
	}

	public GetVmoServicesListCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
