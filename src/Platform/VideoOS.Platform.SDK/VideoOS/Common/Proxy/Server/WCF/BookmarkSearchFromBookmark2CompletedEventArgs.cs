using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class BookmarkSearchFromBookmark2CompletedEventArgs : AsyncCompletedEventArgs, IBookmarkSearchFromBookmarkCompletedEventArgs, IAsyncCompletedEventArgs
{
	private object[] results;

	public Bookmark[] Result
	{
		get
		{
			RaiseExceptionIfNecessary();
			return (Bookmark[])results[0];
		}
	}

	IBookmark[] IBookmarkSearchFromBookmarkCompletedEventArgs.Result => Result;

	public BookmarkSearchFromBookmark2CompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
		: base(exception, cancelled, userState)
	{
		this.results = results;
	}
}
