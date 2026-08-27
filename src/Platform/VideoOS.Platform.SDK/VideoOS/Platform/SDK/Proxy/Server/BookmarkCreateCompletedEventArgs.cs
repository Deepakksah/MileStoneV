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
public class BookmarkCreateCompletedEventArgs : AsyncCompletedEventArgs, IBookmarkCreateCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	IBookmark IBookmarkCreateCompletedEventArgs.Result => Result;

	public Bookmark Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Bookmark)results[0];
		}
	}

	public BookmarkCreateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
