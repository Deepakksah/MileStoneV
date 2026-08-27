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
public class GetSystemLicenseInfoCompletedEventArgs : AsyncCompletedEventArgs, IGetSystemLicenseInfoCompletedEventArgs
{
	private object[] results;

	public SystemLicenseInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SystemLicenseInfo)results[0];
		}
	}

	ISystemLicenseInfo IGetSystemLicenseInfoCompletedEventArgs.Result => Result;

	public GetSystemLicenseInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
