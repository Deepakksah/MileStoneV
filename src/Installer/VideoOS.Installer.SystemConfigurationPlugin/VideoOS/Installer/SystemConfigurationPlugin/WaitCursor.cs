using System;
using System.Windows.Input;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class WaitCursor : IDisposable
{
	private Cursor _previousCursor;

	private bool _disposed;

	public WaitCursor()
	{
		_previousCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = Cursors.Wait;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (!_disposed)
			{
				Mouse.OverrideCursor = _previousCursor;
			}
			_disposed = true;
		}
	}
}
