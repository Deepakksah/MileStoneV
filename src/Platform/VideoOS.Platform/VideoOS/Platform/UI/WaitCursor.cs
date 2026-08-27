using System;
using System.Windows.Forms;

namespace VideoOS.Platform.UI;

public class WaitCursor : IDisposable
{
	private Cursor _oldCursor;

	public WaitCursor()
		: this(Cursors.WaitCursor)
	{
	}

	public WaitCursor(Cursor newCursor)
	{
		_oldCursor = Cursor.Current;
		Cursor.Current = newCursor;
	}

	public void Dispose()
	{
		Dispose(all: true);
	}

	protected virtual void Dispose(bool all)
	{
		Cursor.Current = _oldCursor;
	}
}
