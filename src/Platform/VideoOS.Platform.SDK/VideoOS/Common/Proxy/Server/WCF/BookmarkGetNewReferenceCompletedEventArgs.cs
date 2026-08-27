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
public class BookmarkGetNewReferenceCompletedEventArgs : AsyncCompletedEventArgs, IBookmarkGetNewReferenceCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public BookmarkReference Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (BookmarkReference)results[0];
		}
	}

	IBookmarkReference IBookmarkGetNewReferenceCompletedEventArgs.Result => Result;

	public BookmarkGetNewReferenceCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
