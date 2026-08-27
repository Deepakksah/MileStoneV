using System;
using System.Collections.Generic;

namespace VideoOS.Central.Util;

public class DebouncerEventArgs<TEventArgs> : EventArgs
{
	public IEnumerable<TEventArgs> eventArgs { get; set; }
}
