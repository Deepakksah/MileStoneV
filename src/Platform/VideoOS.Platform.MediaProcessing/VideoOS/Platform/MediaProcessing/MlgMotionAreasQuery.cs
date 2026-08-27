using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class MlgMotionAreasQuery : IDisposable
{
	private unsafe CmMlgMotionBoundingBoxes* m_pQuery;

	public unsafe MlgMotionAreasQuery(bool[] selectionMask, int selectionMaskWidth, int selectionMaskHeight, double level)
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
			if (selectionMask.GetUpperBound(0) + 1 == num2 && !(level < 0.0) && !(level > 1.0))
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
					CmMlgMotionBoundingBoxes* ptr2 = (CmMlgMotionBoundingBoxes*)global::_003CModule_003E.@new(48uL);
					CmMlgMotionBoundingBoxes* ptr3;
					try
					{
						ptr3 = ((ptr2 == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmMlgMotionBoundingBoxes_002E_007Bctor_007D(ptr2, ptr, (ulong)selectionMaskWidth, (ulong)selectionMaskHeight, level));
						CmMlgMotionBoundingBoxes* ptr4 = ptr3;
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr2, 48uL);
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

	private void _007EMlgMotionAreasQuery()
	{
	}

	private unsafe void _0021MlgMotionAreasQuery()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmMlgMotionBoundingBoxes* pQuery = m_pQuery;
		if (pQuery != null)
		{
			long num = *(long*)(*(ulong*)pQuery);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pQuery, 1u);
		}
		m_pQuery = null;
	}

	public unsafe MotionArea[] GetMotionAreas(IReadOnlyBuffer gridData)
	{
		//Discarded unreachable code: IL_0184
		//IL_0009: Expected I8, but got I
		//IL_00bd: Expected I, but got I8
		//IL_0091: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (gridData == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E obj);
		*(long*)(&obj) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16)) = 0L;
		MotionArea[] array;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmMlgMotionBoundingBoxes* native = GetNative();
				ulong length = gridData.Length;
				byte* data = gridData.Data;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E obj2);
				vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E* right = global::_003CModule_003E.NmMediaProcessing_002ECmMlgMotionBoundingBoxes_002EGetMotionBoundingBoxes(native, &obj2, data, length);
				try
				{
					global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E_002E_003D(&obj, right);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E_002E_007Bdtor_007D), &obj2);
					throw;
				}
				global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E_002E_Tidy(&obj2);
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
			long num3 = System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - *(long*)(&obj) >> 4;
			array = new MotionArea[(int)num3];
			int num4 = 0;
			if (0 < (int)num3)
			{
				long num5 = 0L;
				do
				{
					int upperLeftColumn = *(int*)(*(long*)(&obj) + num5 + 4);
					array[num4].UpperLeftColumn = upperLeftColumn;
					int upperLeftRow = *(int*)(num5 + *(long*)(&obj));
					array[num4].UpperLeftRow = upperLeftRow;
					int width = *(int*)(*(long*)(&obj) + num5 + 8);
					array[num4].Width = width;
					int height = *(int*)(*(long*)(&obj) + num5 + 12);
					array[num4].Height = height;
					num4++;
					num5 += 16;
				}
				while (num4 < (int)(System.Runtime.CompilerServices.Unsafe.As<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - *(long*)(&obj) >> 4));
			}
			GC.KeepAlive(gridData);
			GC.KeepAlive(this);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Evector_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_002Cstd_003A_003Aallocator_003CNmMediaProcessing_003A_003ACmMlgMotionBoundingBoxes_003A_003ASmBoundingBox_003E_0020_003E_002E_Tidy(&obj);
		return array;
	}

	public unsafe CmMlgMotionBoundingBoxes* GetNative()
	{
		CmMlgMotionBoundingBoxes* pQuery = m_pQuery;
		if (pQuery == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.MlgMotionAreasQuery");
		}
		return pQuery;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EMlgMotionAreasQuery();
			return;
		}
		try
		{
			_0021MlgMotionAreasQuery();
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

	~MlgMotionAreasQuery()
	{
		Dispose(A_0: false);
	}
}
