using System;
using System.Windows.Input;

namespace VideoOS.Platform.UI;

public class WpfWaitCursor : IDisposable
{
	private Cursor _oldCursor;

	public WpfWaitCursor()
		: this(Cursors.Wait)
	{
	}

	public WpfWaitCursor(Cursor newCursor)
	{
		_oldCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = newCursor;
	}

	public void Dispose()
	{
		Dispose(all: true);
	}

	protected virtual void Dispose(bool all)
	{
		Mouse.OverrideCursor = _oldCursor;
	}
}
