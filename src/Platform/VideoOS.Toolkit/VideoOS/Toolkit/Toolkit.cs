using System;
using NmToolkit;

namespace VideoOS.Toolkit;

public unsafe class Toolkit(ImToolkit* toolkit) : IToolkit
{
	private unsafe ImToolkit* m_Toolkit = toolkit;

	public unsafe virtual void Break()
	{
		//IL_0011: Expected I, but got I8
		ImToolkit* toolkit = m_Toolkit;
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void>)(*(ulong*)(*(long*)toolkit + 8)))((nint)toolkit);
		GC.KeepAlive(this);
	}

	public unsafe ImToolkit* GetNativePtr()
	{
		return m_Toolkit;
	}
}
