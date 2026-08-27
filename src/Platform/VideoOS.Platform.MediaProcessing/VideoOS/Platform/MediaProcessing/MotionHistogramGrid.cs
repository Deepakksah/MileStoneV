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

public class MotionHistogramGrid : IDisposable
{
	private unsafe CmMotionHistogramGrid* m_pDetector;

	public static int NoOfBins;

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
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetEndTime(GetNative(), &cmTimeStamp);
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
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetBeginTime(GetNative(), &cmTimeStamp);
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
			int result = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetHeight(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe int Width
	{
		get
		{
			int result = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetWidth(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe bool Valid
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			bool result = global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EIsValid(GetNative());
			GC.KeepAlive(this);
			return result;
		}
	}

	public unsafe MotionHistogramGrid(CmMotionHistogramGrid* mhg)
	{
		//IL_0008: Expected I, but got I8
		m_pDetector = null;
		base._002Ector();
		m_pDetector = mhg;
		GC.KeepAlive(this);
	}

	public unsafe MotionHistogramGrid()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pDetector = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmMotionHistogramGrid* ptr = (CmMotionHistogramGrid*)global::_003CModule_003E.@new(40uL);
			CmMotionHistogramGrid* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002E_007Bctor_007D(ptr));
				CmMotionHistogramGrid* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 40uL);
				throw;
			}
			m_pDetector = ptr2;
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

	private void _007EMotionHistogramGrid()
	{
		_0021MotionHistogramGrid();
	}

	private unsafe void _0021MotionHistogramGrid()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmMotionHistogramGrid* pDetector = m_pDetector;
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
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EInvalidate(GetNative());
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

	public unsafe void Update(Image oldImage, Image newImage, int width, int height)
	{
		//IL_0008: Expected I8, but got I
		//IL_0086: Expected I, but got I8
		//IL_005c: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (oldImage != null && newImage != null)
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmMotionHistogramGrid* native = GetNative();
				CmImage* native2 = newImage.GetNative();
				CmImage* native3 = oldImage.GetNative();
				global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EUpdate(native, native3, native2, (ulong)width, (ulong)height);
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

	public unsafe void SetData(IReadOnlyBuffer gridData, DateTime beginTime, DateTime endTime)
	{
		//IL_0008: Expected I8, but got I
		//IL_00cb: Expected I, but got I8
		//IL_00a1: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (gridData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmMotionHistogramGrid* native = GetNative();
			System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
			NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp, &endTime);
			try
			{
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp2);
				NmTimeStamp.CmTimeStamp* ptr2 = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_002Cclass_0020System_003A_003ADateTime_003E(&cmTimeStamp2, &beginTime);
				try
				{
					ulong length = gridData.Length;
					byte* data = gridData.Data;
					global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EDeserialize(native, data, length, ptr2, ptr);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp2);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp2);
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
		GC.KeepAlive(gridData);
		GC.KeepAlive(this);
	}

	public unsafe int[] GetMotionHistogram(int x, int y)
	{
		//IL_0008: Expected I8, but got I
		//IL_00e0: Expected I, but got I8
		//IL_00b6: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (x >= 0 && y >= 0)
		{
			int num2 = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetWidth(GetNative());
			GC.KeepAlive(this);
			if (x < num2)
			{
				int num3 = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetHeight(GetNative());
				GC.KeepAlive(this);
				if (y < num3)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
					try
					{
						System.Runtime.CompilerServices.Unsafe.SkipInit(out CmMotionHistogramGrid.MotionHistogram motionHistogram);
						global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetMotionHistogram(GetNative(), (ulong)x, (ulong)y, &motionHistogram);
						int[] array = new int[256];
						IntPtr source = new IntPtr(&motionHistogram);
						Marshal.Copy(source, array, 0, 256);
						GC.KeepAlive(array);
						GC.KeepAlive(this);
						return array;
					}
					catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
					{
						uint num4 = 0u;
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
								num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
								return (byte)num4 != 0;
							}).Invoke())
							{
							}
							if (num4 != 0)
							{
								throw;
							}
						}
						finally
						{
							global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
						}
					}
					return null;
				}
			}
		}
		throw new ArgumentException();
	}

	public unsafe int GetAccumulatedCount(int x, int y, int bin)
	{
		//IL_0008: Expected I8, but got I
		//IL_00be: Expected I, but got I8
		//IL_0094: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (x >= 0 && y >= 0)
		{
			int num2 = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetWidth(GetNative());
			GC.KeepAlive(this);
			if (x < num2)
			{
				int num3 = (int)global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetHeight(GetNative());
				GC.KeepAlive(this);
				if (y < num3 && (uint)bin <= 255u)
				{
					System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
					try
					{
						uint result = global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002EGetAccumulatedCount(GetNative(), (ulong)x, (ulong)y, (ulong)bin);
						GC.KeepAlive(this);
						return (int)result;
					}
					catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
					{
						uint num4 = 0u;
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
								num4 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
								return (byte)num4 != 0;
							}).Invoke())
							{
							}
							if (num4 != 0)
							{
								throw;
							}
						}
						finally
						{
							global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num4);
						}
					}
					return 0;
				}
			}
		}
		throw new ArgumentException();
	}

	public unsafe ReadOnlySharedBuffer GetData([MarshalAs(UnmanagedType.U1)] bool compress)
	{
		//IL_0008: Expected I8, but got I
		//IL_00c6: Expected I, but got I8
		//IL_009b: Expected I, but got I8
		//IL_000c: Expected I, but got I8
		//IL_002d: Expected I, but got I8
		//IL_0035: Expected I, but got I8
		//IL_0043: Expected I4, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			byte* ptr = null;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ulong num2);
			global::_003CModule_003E.NmMediaProcessing_002ECmMotionHistogramGrid_002ESerialize(GetNative(), &ptr, &num2, compress);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
			global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bctor_007D(&cmUniqueBuffer, num2);
			ReadOnlySharedBuffer readOnlySharedBuffer;
			try
			{
				byte* ptr2 = (byte*)System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 80));
				byte* ptr3 = (byte*)System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 80));
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 80)), ptr, num2);
				CmUniqueBuffer* ptr4 = &cmUniqueBuffer;
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
			uint num3 = 0u;
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
		return null;
	}

	public unsafe CmMotionHistogramGrid* GetNative()
	{
		CmMotionHistogramGrid* pDetector = m_pDetector;
		if (pDetector == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.MotionHistogramGrid");
		}
		return pDetector;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EMotionHistogramGrid();
			return;
		}
		try
		{
			_0021MotionHistogramGrid();
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

	~MotionHistogramGrid()
	{
		Dispose(A_0: false);
	}

	static MotionHistogramGrid()
	{
		NoOfBins = 256;
	}
}
