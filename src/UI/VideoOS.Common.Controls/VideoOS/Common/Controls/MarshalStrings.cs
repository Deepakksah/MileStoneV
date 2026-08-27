using System;
using System.Runtime.InteropServices;

namespace VideoOS.Common.Controls;

public sealed class MarshalStrings : IDisposable
{
	private IntPtr _taskAlloc;

	private readonly int _length;

	private IntPtr[] _strings;

	private bool _disposed;

	public IntPtr ArrayPtr => _taskAlloc;

	public MarshalStrings(string[] theArray)
	{
		int size = IntPtr.Size;
		int num = 0;
		if (theArray != null)
		{
			_length = theArray.Length;
			_strings = new IntPtr[_length];
			num = _length * size;
			_taskAlloc = Marshal.AllocCoTaskMem(num);
			for (int num2 = _length - 1; num2 >= 0; num2--)
			{
				_strings[num2] = Marshal.StringToCoTaskMemUni(theArray[num2]);
				Marshal.WriteIntPtr(_taskAlloc, num2 * size, _strings[num2]);
			}
		}
	}

	~MarshalStrings()
	{
		Dispose();
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			if (_taskAlloc != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(_taskAlloc);
				int length = _length;
				while (length-- != 0)
				{
					Marshal.FreeCoTaskMem(_strings[length]);
				}
			}
			_disposed = true;
		}
		GC.SuppressFinalize(this);
	}
}
