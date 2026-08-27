using System;
using System.Windows.Forms;

namespace VideoOS.Installer.Common.Utilities;

public class ShowWaitCursor : IDisposable
{
	private Cursor _current;

	public ShowWaitCursor()
	{
		_current = Cursor.Current;
		Cursor.Current = Cursors.WaitCursor;
	}

	public void Dispose()
	{
		Cursor.Current = _current;
	}
}
