using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmBuffers;
using NmMediaProcessing;
using NmTimeStamp;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MotionLevelGrid : IDisposable
{
	private unsafe CmMotionLevelGrid* m_pDetector = null;

	public static int NoOfBins = 256;

	public unsafe DateTime EndTime
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_008d: Expected I, but got I8
			//IL_0063: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			DateTime result = default(DateTime);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EGetEndTime(GetNative(), &cmTimeStamp);
				try
				{
					result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(&cmTimeStamp);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
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
	}

	public unsafe DateTime BeginTime
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_008d: Expected I, but got I8
			//IL_0063: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			DateTime result = default(DateTime);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EGetBeginTime(GetNative(), &cmTimeStamp);
				try
				{
					result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(&cmTimeStamp);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
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
	}

	public unsafe int Height
	{
		get
		{
			int result = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EGetHeight(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe int Width
	{
		get
		{
			int result = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EGetWidth(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool Valid
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EIsValid(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe MotionLevelGrid(CmMotionLevelGrid* mhg)
	{
		//IL_0008: Expected I, but got I8
		m_pDetector = mhg;
		GC.KeepAlive(this);
	}

	private void _007EMotionLevelGrid()
	{
		_0021MotionLevelGrid();
	}

	private unsafe void _0021MotionLevelGrid()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmMotionLevelGrid* pDetector = m_pDetector;
		if (pDetector != null)
		{
			long num = *(long*)(*(ulong*)pDetector);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pDetector, 1u);
		}
		m_pDetector = null;
	}

	public unsafe void Invalidate()
	{
		//IL_0008: Expected I8, but got I
		//IL_0063: Expected I, but got I8
		//IL_0039: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EInvalidate(GetNative());
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
	}

	public unsafe void Update(Image oldImage, Image newImage, int width, int height, double sensitivity)
	{
		//IL_0008: Expected I8, but got I
		//IL_0088: Expected I, but got I8
		//IL_005e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmMotionLevelGrid* native = GetNative();
				CmImage* native2 = newImage.GetNative();
				CmImage* native3 = oldImage.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002EUpdate(native, native3, native2, (ulong)width, (ulong)height, sensitivity);
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
			GC.KeepAlive(oldImage);
			GC.KeepAlive(newImage);
			GC.KeepAlive(this);
			return;
		}
		throw new ArgumentNullException();
	}

	public unsafe ReadOnlySharedBuffer GetData([MarshalAs(UnmanagedType.U1)] bool compress)
	{
		//IL_0008: Expected I8, but got I
		//IL_0099: Expected I, but got I8
		//IL_006f: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionLevelGrid_002ESerialize(GetNative(), &cmUniqueBuffer, compress);
			ReadOnlySharedBuffer readOnlySharedBuffer;
			try
			{
				CmUniqueBuffer* ptr = &cmUniqueBuffer;
				readOnlySharedBuffer = new ReadOnlySharedBuffer((CmUniqueBuffer*)(&cmUniqueBuffer));
				GC.KeepAlive(this);
				ReadOnlySharedBuffer readOnlySharedBuffer2 = readOnlySharedBuffer;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
				throw;
			}
			global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((std.unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
			return readOnlySharedBuffer;
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
		return null;
	}

	public unsafe CmMotionLevelGrid* GetNative()
	{
		CmMotionLevelGrid* pDetector = m_pDetector;
		if (pDetector == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.MotionLevelGrid");
		}
		return pDetector;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EMotionLevelGrid();
			return;
		}
		try
		{
			_0021MotionLevelGrid();
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

	~MotionLevelGrid()
	{
		Dispose(A_0: false);
	}
}
