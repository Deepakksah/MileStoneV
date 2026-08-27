using System;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

public class WaitCursor : IDisposable
{
	private Cursor oldCursor;

	public WaitCursor()
		: this(Cursors.WaitCursor)
	{
	}

	public WaitCursor(Cursor newCursor)
	{
		oldCursor = Cursor.Current;
		Cursor.Current = newCursor;
	}

	public void Dispose()
	{
		Cursor.Current = oldCursor;
	}
}
