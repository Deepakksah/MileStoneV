using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[ExcludeFromCodeCoverage]
public class GetSystemLicenseInfoCompletedEventArgs : AsyncCompletedEventArgs, IGetSystemLicenseInfoCompletedEventArgs
{
	private object[] results;

	ISystemLicenseInfo IGetSystemLicenseInfoCompletedEventArgs.Result => Result;

	public SystemLicenseInfo Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (SystemLicenseInfo)results[0];
		}
	}

	public GetSystemLicenseInfoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
