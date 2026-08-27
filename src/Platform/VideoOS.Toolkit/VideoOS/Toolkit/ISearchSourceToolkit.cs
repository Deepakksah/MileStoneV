using System;
using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface ISearchSourceToolkit : ISourceToolkit
{
	void StartSearch(DateTime begin, DateTime end, uint maxResults);

	void StopSearch();

	[return: MarshalAs(UnmanagedType.U1)]
	bool GetSearchResult(out IData searchResult, TimeSpan timeout);

	[return: MarshalAs(UnmanagedType.U1)]
	bool GetSearchResult(out IData searchResult);
}
