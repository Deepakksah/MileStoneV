using System;
using System.Threading;

namespace VideoOS.Mobile.Utilities;

public class TaskObject<T>
{
	public Action<T> Action { get; set; }

	public Action<T> Failed { get; set; }

	public T State { get; set; }

	public CancellationToken CancellationToken { get; set; }

	public int TimeoutInMs { get; set; }
}
