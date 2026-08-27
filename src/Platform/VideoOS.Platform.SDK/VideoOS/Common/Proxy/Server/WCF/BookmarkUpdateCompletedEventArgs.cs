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
public class BookmarkUpdateCompletedEventArgs : AsyncCompletedEventArgs, IBookmarkUpdateCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public Bookmark Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Bookmark)results[0];
		}
	}

	IBookmark IBookmarkUpdateCompletedEventArgs.Result => Result;

	public BookmarkUpdateCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
