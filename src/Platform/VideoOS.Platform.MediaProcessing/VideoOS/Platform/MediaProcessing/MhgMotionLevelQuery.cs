using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MhgMotionLevelQuery : IDisposable
{
	private unsafe CmMhgMotionLevelQuery* m_pQuery;

	public unsafe MhgMotionLevelQuery(bool[] selectionMask, int selectionMaskWidth, int selectionMaskHeight, double sensitivity)
	{
		//IL_0009: Expected I8, but got I
		//IL_0011: Expected I, but got I8
		//IL_010e: Expected I, but got I8
		//IL_00d9: Expected I, but got I8
		//IL_005b: Expected I, but got I8
		//IL_009b: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pQuery = null;
		base._002Ector();
		if (selectionMask.GetLowerBound(0) == 0)
		{
			int num2 = selectionMaskWidth * selectionMaskHeight;
			if (selectionMask.GetUpperBound(0) + 1 == num2 && !(sensitivity < 0.0) && !(sensitivity > 1.0))
			{
				bool* ptr = null;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
				try
				{
					ptr = (bool*)global::_003CModule_003E.new_005B_005D((ulong)num2);
					for (int i = 0; i != num2; i++)
					{
						*(bool*)((ulong)i + (ulong)(nint)ptr) = selectionMask[i];
					}
					CmMhgMotionLevelQuery* ptr2 = (CmMhgMotionLevelQuery*)global::_003CModule_003E.@new(64uL);
					CmMhgMotionLevelQuery* ptr3;
					try
					{
						ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMhgMotionLevelQuery_002E_007Bctor_007D(ptr2, ptr, (ulong)selectionMaskWidth, (ulong)selectionMaskHeight, sensitivity));
						CmMhgMotionLevelQuery* ptr4 = ptr3;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr2, 64uL);
						throw;
					}
					m_pQuery = ptr3;
				}
				catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
				{
					uint num3 = 0u;
					global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
					try
					{
						try
						{
							bool* ptr5 = ptr;
							global::_003CModule_003E.delete_005B_005D(ptr);
							throw new Error((exception*)error);
						}
						catch when (((Func<bool>)delegate
						{
							// Could not convert BlockContainer to single expression
							num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
							return (byte)num3 != 0;
						}).Invoke())
						{
						}
						if (num3 != 0)
						{
							throw;
						}
					}
					finally
					{
						global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
					}
				}
				global::_003CModule_003E.delete_005B_005D(ptr);
				GC.KeepAlive(selectionMask);
				GC.KeepAlive(this);
				return;
			}
		}
		throw new ArgumentException();
	}

	private void _007EMhgMotionLevelQuery()
	{
		_0021MhgMotionLevelQuery();
	}

	private unsafe void _0021MhgMotionLevelQuery()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmMhgMotionLevelQuery* pQuery = m_pQuery;
		if (pQuery != null)
		{
			long num = *(long*)(*(ulong*)pQuery);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pQuery, 1u);
		}
		m_pQuery = null;
	}

	public unsafe double MotionLevel(IReadOnlyBuffer gridData)
	{
		//IL_0008: Expected I8, but got I
		//IL_0082: Expected I, but got I8
		//IL_0058: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (gridData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out double result);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmMhgMotionLevelQuery* native = GetNative();
			ulong length = gridData.Length;
			byte* data = gridData.Data;
			result = global::_003CModule_003E.NmMediaProcessing_002ECmMhgMotionLevelQuery_002EMotionLevel(native, data, length);
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num2 != 0;
				}).Invoke())
				{
				}
				if (num2 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
			}
		}
		GC.KeepAlive(gridData);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe CmMhgMotionLevelQuery* GetNative()
	{
		CmMhgMotionLevelQuery* pQuery = m_pQuery;
		if (pQuery == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.MhgMotionLevelQuery");
		}
		return pQuery;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EMhgMotionLevelQuery();
			return;
		}
		try
		{
			_0021MhgMotionLevelQuery();
		}
		finally
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}

	~MhgMotionLevelQuery()
	{
		Dispose(A_0: false);
	}
}
