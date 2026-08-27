using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[Guid("E3652DDB-FA99-44b7-8A28-41F77719CEBF")]
public sealed class ExceptionMessageBoxParent : IWin32Window
{
	private IntPtr _hwnd;

	public IntPtr Handle => _hwnd;

	public ExceptionMessageBoxParent(IntPtr hwnd)
	{
		_hwnd = hwnd;
	}
}
