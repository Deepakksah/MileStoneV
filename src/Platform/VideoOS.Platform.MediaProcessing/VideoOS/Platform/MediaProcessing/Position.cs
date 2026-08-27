using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class Position : IDisposable
{
	protected unsafe CmSimpleImageRegion.CmPosition* m_pPosition;

	public unsafe Position(CmSimpleImageRegion.CmPosition* position)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0087: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		//IL_002f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pPosition = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion.CmPosition* ptr = (CmSimpleImageRegion.CmPosition*)global::_003CModule_003E.@new(16uL);
			CmSimpleImageRegion.CmPosition* pPosition;
			if (ptr != null)
			{
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlock(ptr, position, 16);
				pPosition = ptr;
			}
			else
			{
				pPosition = null;
			}
			m_pPosition = pPosition;
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
	}

	public unsafe Position(uint position, uint range)
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0099: Expected I, but got I8
		//IL_006e: Expected I, but got I8
		//IL_0032: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pPosition = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleImageRegion.CmPosition* ptr = (CmSimpleImageRegion.CmPosition*)global::_003CModule_003E.@new(16uL);
			CmSimpleImageRegion.CmPosition* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ECmPosition_002E_007Bctor_007D(ptr, position, range));
				CmSimpleImageRegion.CmPosition* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 16uL);
				throw;
			}
			m_pPosition = ptr2;
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
	}

	private void _007EPosition()
	{
		_0021Position();
	}

	private unsafe void _0021Position()
	{
		//IL_0016: Expected I, but got I8
		global::_003CModule_003E.delete(m_pPosition, 16uL);
		m_pPosition = null;
	}

	public unsafe Position GetInverted()
	{
		//IL_0008: Expected I8, but got I
		//IL_006d: Expected I, but got I8
		//IL_0043: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		Position result = null;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmSimpleImageRegion.CmPosition cmPosition);
			result = new Position(global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ECmPosition_002EGetInverted(GetNative(), &cmPosition));
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
		GC.KeepAlive(this);
		return result;
	}

	public unsafe uint ToInteger(uint range)
	{
		//IL_0008: Expected I8, but got I
		//IL_0069: Expected I, but got I8
		//IL_003f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		uint result = 0u;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			result = (uint)global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ECmPosition_002EToInteger(GetNative(), range);
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
		GC.KeepAlive(this);
		return result;
	}

	public unsafe double ToDouble()
	{
		double result = global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ECmPosition_002EToDouble(GetNative());
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe static bool operator <(Position lhs, Position rhs)
	{
		if (lhs != null && rhs != null)
		{
			CmSimpleImageRegion.CmPosition* native = lhs.GetNative();
			CmSimpleImageRegion.CmPosition* native2 = rhs.GetNative();
			bool result = global::_003CModule_003E.NmMediaProcessing_002ECmSimpleImageRegion_002ECmPosition_002E_003C(native, native2);
			GC.KeepAlive(lhs);
			GC.KeepAlive(rhs);
			return result;
		}
		throw new ArgumentNullException();
	}

	public unsafe CmSimpleImageRegion.CmPosition* GetNative()
	{
		CmSimpleImageRegion.CmPosition* pPosition = m_pPosition;
		if (pPosition == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.Position");
		}
		return pPosition;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EPosition();
			return;
		}
		try
		{
			_0021Position();
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

	~Position()
	{
		Dispose(A_0: false);
	}
}
