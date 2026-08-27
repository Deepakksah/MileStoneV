using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ExcludeFromCodeCoverage]
public class GetVmoServicesListCompletedEventArgs : AsyncCompletedEventArgs, IGetVmoServicesListCompletedEventArgs
{
	private object[] results;

	public System.Collections.DictionaryEntry[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (System.Collections.DictionaryEntry[])results[0];
		}
	}

	IDictionaryEntry[] IGetVmoServicesListCompletedEventArgs.Result
	{
		get
		{
			DictionaryEntry[] array = new DictionaryEntry[Result.Length];
			for (int i = 0; i < Result.Length; i++)
			{
				DictionaryEntry dictionaryEntry = new DictionaryEntry();
				dictionaryEntry.Key = Result[i].Key;
				dictionaryEntry.Value = Result[i].Value;
				array[i] = dictionaryEntry;
			}
			return array;
		}
	}

	public GetVmoServicesListCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
