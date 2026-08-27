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
public class GetProductInfoCompletedEventArgs : AsyncCompletedEventArgs, IGetProductInfoCompletedEventArgs
{
	private object[] results;

	public ProductInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (ProductInfo)results[0];
		}
	}

	IProductInfo IGetProductInfoCompletedEventArgs.Result => Result;

	public GetProductInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
