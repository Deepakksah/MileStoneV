using System.Collections;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class SortedEnumerable<T> : ISortedEnumerable<T>, IEnumerable<T>, IEnumerable
{
	public IEnumerable<T> WrappedEnumerable { get; private set; }

	public SortedEnumerable(IEnumerable<T> wrappedEnumerable)
	{
		WrappedEnumerable = wrappedEnumerable;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return WrappedEnumerable.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)WrappedEnumerable).GetEnumerator();
	}
}
