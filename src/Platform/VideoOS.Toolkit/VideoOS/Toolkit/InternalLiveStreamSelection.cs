using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public unsafe class InternalLiveStreamSelection(ImStreamSelection* liveStreamSelection) : LiveStreamSelection
{
	private unsafe ImStreamSelection* m_LiveStreamSelection = liveStreamSelection;

	private void _007EInternalLiveStreamSelection()
	{
		_0021InternalLiveStreamSelection();
	}

	private unsafe void _0021InternalLiveStreamSelection()
	{
		//IL_0018: Expected I, but got I8
		ImStreamSelection* liveStreamSelection = m_LiveStreamSelection;
		if (liveStreamSelection != null)
		{
			ImStreamSelection* ptr = liveStreamSelection;
			long num = *(long*)(*(ulong*)ptr);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)ptr, 1u);
		}
		GC.KeepAlive(this);
	}

	public unsafe ImStreamSelection* CloneSelection()
	{
		//IL_0026: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		ImStreamSelection* liveStreamSelection = m_LiveStreamSelection;
		if (liveStreamSelection != null)
		{
			ImStreamSelection* result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImStreamSelection*>)(*(ulong*)(*(long*)liveStreamSelection + 8)))((nint)liveStreamSelection);
			GC.KeepAlive(this);
			return result;
		}
		GC.KeepAlive(this);
		return null;
	}

	[HandleProcessCorruptedStateExceptions]
	protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			try
			{
				_0021InternalLiveStreamSelection();
				return;
			}
			finally
			{
				base.Dispose(A_0: true);
			}
		}
		try
		{
			_0021InternalLiveStreamSelection();
		}
		finally
		{
			base.Dispose(A_0: false);
		}
	}

	~InternalLiveStreamSelection()
	{
		Dispose(A_0: false);
	}
}
